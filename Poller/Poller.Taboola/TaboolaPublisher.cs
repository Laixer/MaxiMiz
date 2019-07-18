using System;
using System.Threading;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Poller.Publisher;
using Poller.Scheduler;
using Poller.Scheduler.Delegate;

namespace Poller.Taboola
{
    [Publisher("Taboola")]
    public class TaboolaPublisher : RemotePublisher
    {
        private readonly DbConnection connection;
        private readonly TaboolaPollerOptions options;

        /// <summary>
        /// Creates a TaboolaPoller for fetching Data from Taboola.
        /// </summary>
        /// <param name="logger">A logger for this poller.</typeparam>
        /// <param name="options">An instance of options required for requests.</param>
        /// <param name="connection">The database connections to use for inserting fetched data.</param>
        public TaboolaPublisher(ILogger<TaboolaPublisher> logger, IOptions<TaboolaPollerOptions> options, DbConnection connection)
            : base(logger)
        {
            this.options = options?.Value;
            this.connection = connection;
        }

        public override ScheduleCollection CreateSchedulerScheme(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var poller = new TaboolaPoller(Logger, options, connection);
            return new ScheduleCollection
            {
                RefreshAdvertisementDataProvider = new RefreshAdvertisementDataDelegate(poller, TimeSpan.FromMinutes(10)),
                //DataSyncbackProvider = new DataSyncbackDelegate(poller, TimeSpan.FromMinutes(3)),
                //CreateOrUpdateObjectsProvider = new CreateOrUpdateObjectsDelegate(poller, TimeSpan.FromMinutes(10)),
            };
        }
    }
}
