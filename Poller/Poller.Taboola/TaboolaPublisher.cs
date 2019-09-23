using System;
using System.Threading;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Poller.Publisher;
using Poller.Database;
using Poller.Scheduler.Activator;

namespace Poller.Taboola
{

    /// <summary>
    /// Our Taboola publisher class. This sets up our poller
    /// to communicate with Taboola.
    /// </summary>
    [Publisher("Taboola")]
    public class TaboolaPublisher : IRemotePublisher, IDisposable
    {
        private readonly TaboolaPoller _poller;
        private readonly ActivatorFactory _activatorFactory;
        private readonly TaboolaPollerOptions _options;

        /// <summary>
        /// Constructor with dependency injection.
        /// </summary>
        /// <param name="logger">A logger for this poller.</param>
        /// <param name="options">An instance of options required for requests.</param>
        /// <param name="provider">The database provider to use for inserting fetched data.</param>
        /// <param name="cache">The cache</param>
        /// <param name="activatorFactory">The activator factory</param>
        public TaboolaPublisher(ILoggerFactory logger, IOptions<TaboolaPollerOptions> options,
            DbProvider provider, IMemoryCache cache, ActivatorFactory activatorFactory)
        {
            _poller = new TaboolaPoller(logger, options?.Value, provider, cache);
            _activatorFactory = activatorFactory;
            _options = options?.Value;
        }

        /// <summary>
        /// Returns all activators for this poller. Each activator
        /// will perform some desired action upon trigger, either
        /// by time or by some event. The actual functionality of
        /// the poller is determined by these activators.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The list of activators</returns>
        public IEnumerable<ActivatorBase> GetActivators(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return new Collection<ActivatorBase>
            {
                _activatorFactory.TimeActivator(new RefreshAdvertisementDataDelegate(_poller), TimeSpan.FromMinutes(_options.RefreshAdvertisementDataInterval)),
                _activatorFactory.TimeActivator(new DataSyncbackDelegate(_poller), TimeSpan.FromMinutes(_options.DataSyncbackInterval)),
                _activatorFactory.EventActivator(new CreateOrUpdateObjectsDelegate(_poller), _options.CreateOrUpdateObjectsEventBus),
            };
        }

        /// <summary>
        /// Called upon safe shutdown.
        /// </summary>
        public void Dispose() => _poller?.Dispose();
    }
}
