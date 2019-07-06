using System.Threading.Tasks;
using MaxiMiz.Poller.Model.Response;

namespace Poller.Publisher
{
    public interface IRemotePublisher
    {
        Task<TopCampaignReport> GetTopCampaignReportAsync();
    }
}
