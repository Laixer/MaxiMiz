using System.Threading.Tasks;

namespace Poller.Publisher
{
    public interface IRemotePublisher
    {
        Task RefreshAdvertisementDataAsync();
    }
}
