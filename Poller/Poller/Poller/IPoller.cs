using System.Threading;
using System.Threading.Tasks;

namespace Poller.Poller
{
    public interface IPoller
    {
    }

    public interface IPollerRefreshAdvertisementData : IPoller
    {
        Task RefreshAdvertisementDataAsync(PollerContext contex, CancellationToken token);
    }

    public interface IPollerDataSyncback : IPoller
    {
        Task DataSyncbackAsync(CancellationToken token);
    }

    public interface IPollerCreateOrUpdateObjects : IPoller
    {
        Task CreateOrUpdateObjectsAsync(CancellationToken token);
    }
}
