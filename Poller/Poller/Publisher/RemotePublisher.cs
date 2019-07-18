using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Poller.Publisher
{
    public abstract class RemotePublisher : IRemotePublisher
    {
        public ILogger Logger { get; }

        public RemotePublisher(ILogger<RemotePublisher> logger)
        {
            Logger = logger;
        }

        public abstract Task RefreshAdvertisementDataAsync();
    }
}
