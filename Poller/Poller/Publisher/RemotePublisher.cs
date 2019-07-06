using System.Threading.Tasks;
using MaxiMiz.Poller.Model.Response;
using Microsoft.Extensions.Configuration;
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

        public abstract Task<TopCampaignReport> GetTopCampaignReportAsync();
    }
}
