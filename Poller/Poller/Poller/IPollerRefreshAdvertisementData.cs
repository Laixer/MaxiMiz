using System.Threading;
using System.Threading.Tasks;

namespace Poller.Poller
{

    /// <summary>
    /// Used to refresh ad item data and retreive numeric performance values.
    /// This is not used to check if our internal data is still up to date with
    /// the external data, that is done in the <see cref="IPollerDataSyncback"/>
    /// interface.
    /// </summary>
    public interface IPollerRefreshAdvertisementData : IPoller
    {

        /// <summary>
        /// Retreives numeric performance values for our ad items.
        /// </summary>
        /// <param name="contex">The poller context</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        Task RefreshAdvertisementDataAsync(PollerContext contex, CancellationToken token);

    }
}
