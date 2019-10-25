using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Poller.FunctionHost.Taboola.Utility;
using Poller.Poller;
using Poller.Taboola;
using System.Threading;
using System.Threading.Tasks;

namespace Poller.FunctionHost.Taboola
{

    /// <summary>
    /// Azure function for the <see cref="IPollerCreateOrUpdateObjects"/> 
    /// interface for the Taboola poller.
    /// </summary>
    public class TaboolaCreateOrUpdateObjects
    {

        /// <summary>
        /// The injected poller object.
        /// </summary>
        private readonly TaboolaPoller _poller;

        /// <summary>
        /// The poller context singleton.
        /// TODO Clean up singleton
        /// </summary>
        private static PollerContext _pollerContext;
        private static PollerContext pollerContext
        {
            get
            {
                if (_pollerContext == null)
                {
                    _pollerContext = new PollerContext(0, null);
                }
                return _pollerContext;
            }
        }

        /// <summary>
        /// Constructor for DI.
        /// </summary>
        /// <param name="poller">Injected taboola poller object</param>
        public TaboolaCreateOrUpdateObjects(TaboolaPoller poller)
        {
            _poller = poller;
        }

        /// <summary>
        /// Azure function that gets triggered by ou CRUD service bus.
        /// TODO If we fail we should process that information!
        /// </summary>
        /// <remarks>
        /// This handles and displays all internal exceptions.
        /// </remarks>
        /// <param name="queueItem">The recieved queue item as string</param>
        /// <param name="log">The injected logger</param>
        /// <returns>Task</returns>
        [FunctionName("TaboolaCreateOrUpdateObjects")]
        public async Task Run([ServiceBusTrigger("testqueue", Connection = "MaximizServiceBusListen")]string queueItem, ILogger log)
        {
            // Beun assert that queue name is correct
            // TODO

            // Execute with cancellation token and clean up
            // TODO Token is useless here, do this differently
            try
            {
                var context = new ContextParser().Parse(queueItem);

                using (var source = new CancellationTokenSource())
                {
                    await _poller.CreateOrUpdateObjectsAsync(context, source.Token);
                }
            }
            catch (JsonException e)
            {
                log.LogError($"Could not parse service bus message in function {nameof(TaboolaCreateOrUpdateObjects)}.");
                throw;
            }
        }

    }
}
