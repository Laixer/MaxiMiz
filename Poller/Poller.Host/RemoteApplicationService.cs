using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Poller.Publisher;

namespace Poller.Host
{
    internal class RemoteApplicationService : IHostedService, IDisposable
    {
        private readonly RemoteApplicationServiceOptions _options;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly object executionLock = new object();
        private ICollection<IRemotePublisher> _remotePublishers;
        private System.Timers.Timer _timer;

        public ILogger Logger { get; }
        public IServiceProvider Services { get; }
        public CancellationToken CancellationToken { get => _cancellationTokenSource.Token; }

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
        /// Only one session will run at all time. All following threads will skip
        /// execution if the lock is hold.
        /// </remarks>
        /// <param name="cancellationToken">Cancellation token.</param>
        private void RunAllPublishers(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) { return; }

            if (Monitor.TryEnter(executionLock))
            {
                try
                {
                    Logger.LogInformation("Running publishers.");

                    int index = 0;
                    var taskCollection = new Task[_remotePublishers.Count()];
                    foreach (var remotePublishers in _remotePublishers)
                    {
                        taskCollection[index++] = ExecutePublisher(remotePublishers, cancellationToken);
                    }

                    Task.WhenAll(taskCollection).Wait(cancellationToken);
                }
                catch (OperationCanceledException) { }
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
        /// <returns></returns>
        private async Task ExecutePublisher(IRemotePublisher publisher, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) { return; }

            await Task.Yield();

            try
            {
                Logger.LogDebug($"Start {publisher.GetType().FullName}.");

                // Link the cancellation tokens into one new cancellation token
                var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(TimeSpan.FromMinutes(_options.PublisherOperationTimeout));

                var cancelToken = cts.Token;
                cancelToken.Register(() =>
                {
                    Logger.LogWarning("Operation timeout, task killed");
                });

                await Task.Run(async () => await publisher.GetTopCampaignReportAsync().ConfigureAwait(false), cancelToken);
            }
            catch (Exception e) when (e as OperationCanceledException == null)
            {
                Logger.LogError(e.Message);
                Logger.LogTrace(e.StackTrace);
            }
            finally
            {
                Logger.LogDebug($"Finish {publisher.GetType().FullName}.");
            }
        }

        /// <summary>
        /// Stop service by cancelling all tokens.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Services stopping");

            _cancellationTokenSource.Cancel();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
