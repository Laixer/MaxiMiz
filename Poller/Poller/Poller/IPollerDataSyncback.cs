using System.Threading;
using System.Threading.Tasks;

namespace Poller.Poller
{

    /// <summary>
    /// Used to pull back data from the external API to keep our internal data
    /// up to date.
    /// </summary>
    public interface IPollerDataSyncback : IPoller
    {
        Task DataSyncbackAsync(PollerContext contex, CancellationToken token);
    }
}
