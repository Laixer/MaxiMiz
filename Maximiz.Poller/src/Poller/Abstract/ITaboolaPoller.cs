using System.Threading.Tasks;
using model.response;

namespace MaxiMiz.Poller.Poller.Abstract
{

    internal interface ITaboolaPoller : IRestPoller
    {
        Task<TopCampaignReport> GetTopCampaignReport();
        Task<string> GetOAuth2Response();
    }
}

