using System;
using System.Threading;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Poller.Publisher;
using Poller.Scheduler.Activator;
using Poller.Scheduler.Delegate;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Poller.Taboola
{
    [Publisher("Taboola")]
    public class TaboolaPublisher : IRemotePublisher, IDisposable
    {
        private readonly TaboolaPoller _poller;
        private readonly ActivatorFactory _activatorFactory;

        /// <summary>
        /// Creates a TaboolaPoller for fetching Data from Taboola.
        /// </summary>
        /// <param name="logger">A logger for this poller.</typeparam>
        /// <param name="options">An instance of options required for requests.</param>
        /// <param name="connection">The database connections to use for inserting fetched data.</param>
        public TaboolaPublisher(
            ILogger<TaboolaPublisher> logger,
            IOptions<TaboolaPollerOptions> options,
            DbConnection connection,
            IMemoryCache cache,
            ActivatorFactory activatorFactory)
        {
            _poller = new TaboolaPoller(logger, options?.Value, connection, cache);
            _activatorFactory = activatorFactory;
        }

        public IEnumerable<ActivatorBase> GetActivators(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return new Collection<ActivatorBase>
            {
                _activatorFactory.TimeActivator(new RefreshAdvertisementDataDelegate(_poller), TimeSpan.FromSeconds(10)),
                _activatorFactory.TimeActivator(new DataSyncbackDelegate(_poller), TimeSpan.FromSeconds(10)),
                _activatorFactory.EventActivator(new CreateOrUpdateObjectsDelegate(_poller)),
            };
        }

        public void Dispose() => _poller?.Dispose();
    }
}
