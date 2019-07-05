using System.IO;
using System.Threading.Tasks;
using MaxiMiz.Poller.Helper;
using MaxiMiz.Poller.Poller;
using MaxiMiz.Poller.Model.Response;

namespace MaxiMiz.Poller
{
    internal class TaboolaPollerMock : PollerBase
    {
        public async override Task<TopCampaignReport> GetTopCampaignReportAsync()
        {
            using (var stream = File.Create(@"mock.json"))
            {
                return await Task.Run(() => Json.Deserialize<TopCampaignReport>(stream));
            }
        }
    }
}
