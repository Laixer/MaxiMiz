using System.Threading;
using System.Threading.Tasks;

namespace Poller.Poller
{
    public interface IPollerRefreshAdvertisementData : IPoller
    {
        Task RefreshAdvertisementDataAsync(PollerContext contex, CancellationToken token);
    }
}
