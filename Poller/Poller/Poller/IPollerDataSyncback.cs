using System.Threading;
using System.Threading.Tasks;

namespace Poller.Poller
{
    public interface IPollerDataSyncback : IPoller
    {
        Task DataSyncbackAsync(PollerContext contex, CancellationToken token);
    }
}
