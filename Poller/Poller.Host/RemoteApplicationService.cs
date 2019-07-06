using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Poller.Publisher;

namespace Poller.Host
{
    internal class RemoteApplicationService : IHostedService, IDisposable
    {
        private readonly static double refreshInterval = 60 * 1000;
        private ICollection<IRemotePublisher> _remotePublishers;
        private System.Timers.Timer _timer;
        private object executionLock = new object();

        public ILogger Logger { get; }
        public IServiceProvider Services { get; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="services">Service provider.</param>
        public RemoteApplicationService(ILogger<RemoteApplicationService> logger, IServiceProvider services)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Services = services ?? throw new ArgumentNullException(nameof(services));

            _timer = new System.Timers.Timer(refreshInterval);
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
                _timer.Elapsed += (s, e) => RunAllPublishers(cancellationToken);
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
        private void RunAllPublishers(CancellationToken cancellationToken)
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
                finally
                {
                    Monitor.Exit(executionLock);
                }
            }
        }

        private async Task ExecutePublisher(IRemotePublisher publisher, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) { return; }

            await Task.Yield();

            try
            {
                Logger.LogDebug($"Start {publisher.GetType().FullName}.");
                await publisher.GetTopCampaignReportAsync();
            }
            catch (Exception e) when (e as OperationCanceledException == null)
            {
                Logger.LogError(e.Message);
            }
            finally
            {
                Logger.LogDebug($"Finish {publisher.GetType().FullName}.");
            }
        }

        /// <summary>
        /// Stop service.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Stopping service...");

            _timer.Stop();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
