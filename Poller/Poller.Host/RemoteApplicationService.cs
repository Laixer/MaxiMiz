using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Poller.Publisher;
using Poller.Scheduler;

namespace Poller.Host
{
    /// <summary>
    /// Run the remote services.
    /// </summary>
    internal class RemoteApplicationService : IHostedService, IDisposable
    {
        private readonly RemoteApplicationServiceOptions _options;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ICollection<Timer> _timers = new List<Timer>();

        protected ILogger Logger { get; }
        protected IServiceProvider Services { get; }
        protected CancellationToken CancellationToken { get => _cancellationTokenSource.Token; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="services">Service provider.</param>
        public RemoteApplicationService(ILogger<RemoteApplicationService> logger, IServiceProvider services, IOptions<RemoteApplicationServiceOptions> options)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Services = services ?? throw new ArgumentNullException(nameof(services));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Schedule the timer for next interval.
        /// </summary>
        /// <remarks>
        /// The scheduler will add a bias offset to the timer for a more 
        /// overal balanced scheme, if the function is configured to do so.
        /// </remarks>
        /// <param name="timer">Configurable timer.</param>
        /// <param name="timeSpan">Requested interval.</param>
        /// <param name="withBias">Optional timespan bias.</param>
        /// <returns>Timer object passed in.</returns>
        private static Timer ScheduleTimer(Timer timer, TimeSpan timeSpan, bool withBias = true)
        {
            Random rand = new Random();
            var timerOffset = TimeSpan.FromSeconds(rand.Next(15, 45));

            timer.Change(withBias
                ? timeSpan.Add(timerOffset)
                : timeSpan, TimeSpan.FromMilliseconds(-1));
            return timer;
        }

        /// <summary>
        /// Context for each scheduled provider.
        /// </summary>
        internal class ProviderContext
        {
            public IOperationDelegate Provider { get; set; }
            public Timer Timer { get; set; }
        }

        /// <summary>
        /// Start service and schedule all operations to run.
        /// </summary>
        /// <remarks>This method will catch *all* exceptions.</remarks>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) { return Task.CompletedTask; }

            Logger.LogInformation("Services starting");

            try
            {
                foreach (var remotePublishers in Services.GetService<IEnumerable<IRemotePublisher>>().ToArray())
                {
                    foreach (var provider in remotePublishers.CreateSchedulerScheme(cancellationToken).AsParallel())
                    {
                        var context = new ProviderContext
                        {
                            Provider = provider,
                        };

                        context.Timer = new Timer(ExecuteProvider,
                            context,
                            TimeSpan.FromMilliseconds(-1),
                            TimeSpan.FromMilliseconds(-1));

                        // Add the timer to the list for later disposal.
                        _timers.Add(ScheduleTimer(context.Timer, provider.Interval));
                    }
                }
            }
            catch { }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Run an operation delegate.
        /// </summary>
        /// <param name="state">An object of <see cref="ProviderContext"/>.</param>
        protected virtual void ExecuteProvider(object state)
        {
            var context = state as ProviderContext;

            if (CancellationToken.IsCancellationRequested) { return; }

            var watch = new Stopwatch();

            try
            {
                Logger.LogInformation($"Start {context.Provider.GetType().FullName}");
                Logger.LogDebug($"Start {context.Provider.GetType().FullName} at {DateTime.Now}");

                watch.Start();

                // Link the cancellation tokens into one new cancellation token, but only for local scope.
                using (var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(CancellationToken))
                using (var deadlineTimer = new Timer((_) => combinedCts.Cancel()))
                {
                    combinedCts.Token.Register(() =>
                    {
                        Logger.LogWarning("Operation timeout or canceled, task killed");
                    });

                    // Restart the timeout from this point.
                    context.Provider.OnSlidingWindowChange += (s, e) =>
                    {
                        // _options.PublisherOperationTimeout
                        // Use provider timespan if timeout is set by provider.
                        deadlineTimer.Change(context.Provider.Timeout.TotalMilliseconds > 0
                            ? context.Provider.Timeout
                            : TimeSpan.FromMinutes(1), TimeSpan.FromMilliseconds(-1));
                    };

                    // _options.PublisherOperationTimeout
                    // Use provider timespan if timeout is set by provider.
                    deadlineTimer.Change(context.Provider.Timeout.TotalMilliseconds > 0
                        ? context.Provider.Timeout
                        : TimeSpan.FromMinutes(1), TimeSpan.FromMilliseconds(-1));

                    // Run synchronous
                    Task.Run(async () =>
                    {
                        await context.Provider.InvokeAsync(combinedCts.Token).ConfigureAwait(false);
                    }, combinedCts.Token).Wait();
                }
            }
            catch (Exception e) when (e as OperationCanceledException == null)
            {
                Logger.LogError(e.Message);
                Logger.LogTrace(e.StackTrace);
            }
            finally
            {
                watch.Stop();

                Logger.LogInformation($"Finished {context.Provider.GetType().FullName}");
                Logger.LogDebug($"Finished {context.Provider.GetType().FullName} in {watch.Elapsed}");

                ScheduleTimer(context.Timer, context.Provider.Interval);

                Logger.LogDebug($"Rerun {context.Provider.GetType().FullName} in ~{context.Provider.Interval}");
            }
        }

        /// <summary>
        /// Stop service by cancelling all tokens.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) { return Task.CompletedTask; }

            Logger.LogInformation("Services stopping");

            _cancellationTokenSource.Cancel();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();

            foreach (var timer in _timers)
            {
                timer.Dispose();
            }
        }
    }
}
