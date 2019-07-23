using System.Threading;
using System.Threading.Tasks;

namespace Poller.Poller
{
    public interface IPollerCreateOrUpdateObjects : IPoller
    {
        Task CreateOrUpdateObjectsAsync(PollerContext contex, CancellationToken token);
    }
}
