using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Poller.Publisher;

namespace Poller.Host
{
    internal class RemoteApplicationService : IHostedService
    {
        private IEnumerable<IRemotePublisher> _remotePublishers;

        public IServiceProvider Services { get; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="services">Service provider.</param>
        public RemoteApplicationService(IServiceProvider services)
        {
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
            foreach (var remotePublishers in _remotePublishers)
            {
                await remotePublishers.GetTopCampaignReportAsync();
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
