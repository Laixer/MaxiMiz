using System.IO;
using System.Threading.Tasks;
using Poller.Helper;
using Poller.Model.Response;
using Poller.Publisher;

namespace Poller.Taboola
{

    public class TaboolaPollerMock : IRemotePublisher
    {
        /// <summary>
        /// Mocks the request to taboola for testing purposes by using a JSON file with a mock response.
        /// </summary>  
        public async Task GetTopCampaignReportAsync()
        {
            using (var stream = File.Create(@"mock.json"))
            {
                await Task.Run(() => Json.Deserialize<TopCampaignReport>(stream));
            }
        }
    }
}
