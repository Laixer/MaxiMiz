using System.IO;
using System.Threading.Tasks;

using MaxiMiz.Poller.Model.Response;
using Poller.Helper;
using Poller.Publisher;

namespace Poller.Taboola
{
    public class TaboolaPollerMock : IRemotePublisher
    {
        public async Task<TopCampaignReport> GetTopCampaignReportAsync()
        {
            using (var stream = File.Create(@"mock.json"))
            {
                return await Task.Run(() => Json.Deserialize<TopCampaignReport>(stream));
            }
        }
    }
}
