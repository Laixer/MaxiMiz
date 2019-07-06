using System.Threading.Tasks;
using MaxiMiz.Poller.Model.Response;
using Microsoft.Extensions.Configuration;

namespace Poller.Publisher
{
    public abstract class RemotePublisher : IRemotePublisher
    {
        public IConfiguration Configuration { get; }

        public RemotePublisher(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public abstract Task<TopCampaignReport> GetTopCampaignReportAsync();
    }
}
