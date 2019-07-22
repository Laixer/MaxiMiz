using System;
using System.Threading;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Poller.Publisher;
using Poller.Scheduler;
using Poller.Scheduler.Delegate;

namespace Poller.Taboola
{
    [Publisher("Taboola")]
    public class TaboolaPublisher : RemotePublisher, IDisposable
    {
        private readonly TaboolaPoller poller;

        /// <summary>
        /// Creates a TaboolaPoller for fetching Data from Taboola.
        /// </summary>
        /// <param name="logger">A logger for this poller.</typeparam>
        /// <param name="options">An instance of options required for requests.</param>
        /// <param name="connection">The database connections to use for inserting fetched data.</param>
        public TaboolaPublisher(ILogger<TaboolaPublisher> logger, IOptions<TaboolaPollerOptions> options, DbConnection connection, IMemoryCache cache)
            : base(logger)
        {
            poller = new TaboolaPoller(Logger, options?.Value, connection, cache);
        }

        public override ScheduleCollection CreateSchedulerScheme(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return new ScheduleCollection
            {
                RefreshAdvertisementDataProvider = new RefreshAdvertisementDataDelegate(poller, TimeSpan.FromSeconds(10)),
                //DataSyncbackProvider = new DataSyncbackDelegate(poller, TimeSpan.FromMinutes(3)),
                //CreateOrUpdateObjectsProvider = new CreateOrUpdateObjectsDelegate(poller, TimeSpan.FromMinutes(10)),
            };
        }

        public void Dispose() => poller?.Dispose();
    }
}
