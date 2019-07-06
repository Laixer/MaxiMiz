using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Poller.Publisher;

namespace Poller.Host
{
    internal class RemoteApplicationService : IHostedService
    {
        private readonly static TimeSpan serviceInterval = TimeSpan.FromMinutes(5);
        private IEnumerable<IRemotePublisher> _remotePublishers;

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
        }

        /// <summary>
        /// Start service.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) { return; }

            _remotePublishers = Services.GetService<IEnumerable<IRemotePublisher>>();
            while (!cancellationToken.IsCancellationRequested)
            {
                Logger.LogInformation("Running publishers.");

                foreach (var remotePublishers in _remotePublishers)
                {
                    try
                    {
                        await remotePublishers.GetTopCampaignReportAsync();
                    }
                    catch (Exception e) when (e as OperationCanceledException == null)
                    {
                        Logger.LogCritical(e.Message);
                    }
                }

                Logger.LogDebug($"Sleeping for {serviceInterval}");

                await Task.Delay(serviceInterval, cancellationToken);
            }
        }

        /// <summary>
        /// Stop service.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
