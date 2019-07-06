using System.Threading.Tasks;
using MaxiMiz.Poller.Model.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Poller.Publisher
{
    public abstract class RemotePublisher : IRemotePublisher
    {
        public ILogger Logger { get; }
        public IConfiguration Configuration { get; }

        public RemotePublisher(ILogger<RemotePublisher> logger, IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
        }

        public abstract Task<TopCampaignReport> GetTopCampaignReportAsync();
    }
}
