using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using MaxiMiz.Poller.Model.Response;
using Poller.Helper;
using Poller.Publisher;

namespace Poller.Taboola
{
    internal class TaboolaPollerMock : RemotePublisher
    {
        public TaboolaPollerMock(IConfiguration configuration)
            : base(configuration)
        { }

        public async override Task<TopCampaignReport> GetTopCampaignReportAsync()
        {
            using (var stream = File.Create(@"mock.json"))
            {
                return await Task.Run(() => Json.Deserialize<TopCampaignReport>(stream));
            }
        }
    }
}
