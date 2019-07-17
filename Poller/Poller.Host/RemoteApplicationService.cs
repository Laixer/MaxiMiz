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

namespace Poller.Host
{
    /// <summary>
    /// Run the remote services.
    /// </summary>
    internal class RemoteApplicationService : IHostedService, IDisposable
    {
        private readonly RemoteApplicationServiceOptions _options;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly object executionLock = new object();
        private readonly System.Timers.Timer _timer;
        private ICollection<IRemotePublisher> _remotePublishers;

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
            _timer = new System.Timers.Timer(_options.StartupDelay * 1000);
            _cancellationTokenSource = new CancellationTokenSource();

            // Stop the timer when cancel is called
            CancellationToken.Register(() =>
            {
                _timer.Stop();
            });
        }

        /// <summary>
        /// Start service.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) { return Task.CompletedTask; }

            _remotePublishers = Services.GetService<IEnumerable<IRemotePublisher>>().ToArray();
            if (_remotePublishers.Count() > 0)
            {
                _timer.Elapsed += (s, e) => RunAllPublishers(CancellationToken);
                _timer.Start();
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Run all publishers.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Only one session will run at all time. All following threads will skip
        ///         execution if the lock is hold.
        ///     </para>
        ///     <para>
        ///         All exceptions are caught and ignored here. Nothing can be thrown from
        ///         this method to prevent the host from shutting down.
        ///     </para>
        /// </remarks>
        /// <param name="cancellationToken">Cancellation token.</param>
        private void RunAllPublishers(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) { return; }

            if (Monitor.TryEnter(executionLock))
            {
                try
                {
                    Logger.LogInformation("Running services");

                    int index = 0;
                    var taskCollection = new Task[_remotePublishers.Count()];
                    foreach (var remotePublishers in _remotePublishers)
                    {
                        taskCollection[index++] = ExecutePublisher(remotePublishers, cancellationToken);
                    }

                    Task.WhenAll(taskCollection).Wait(cancellationToken);
                }
                catch { }
                finally
                {
                    _timer.Interval = _options.PublisherRefreshInterval * 60 * 1000;
                    Monitor.Exit(executionLock);
                }
            }
        }

        /// <summary>
        /// Run each publisher.
        /// </summary>
        /// <param name="publisher">Executing publisher.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        private async Task ExecutePublisher(IRemotePublisher publisher, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) { return; }

            await Task.Yield();

            var watch = new Stopwatch();

            try
            {
                Logger.LogDebug($"Start {publisher.GetType().FullName} at {DateTime.Now}");

                watch.Start();

                // Link the cancellation tokens into one new cancellation token
                var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(TimeSpan.FromMinutes(_options.PublisherOperationTimeout));

                cts.Token.Register(() =>
                {
                    Logger.LogWarning("Operation timeout, task killed");
                });

                await Task.Run(async () => await publisher.GetTopCampaignReportAsync().ConfigureAwait(false), cts.Token);
            }
            catch (Exception e) when (e as OperationCanceledException == null)
            {
                Logger.LogError(e.Message);
                Logger.LogTrace(e.StackTrace);
            }
            finally
            {
                watch.Stop();

                Logger.LogDebug($"Finished {publisher.GetType().FullName} in {watch.Elapsed}");
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
            _timer?.Dispose();
        }
    }
}
