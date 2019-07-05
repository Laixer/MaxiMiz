using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using MaxiMiz.Poller.Helper;
using MaxiMiz.Poller.Poller.Abstract;
using model.response;

namespace MaxiMiz.Poller.Mock
{
    internal class TaboolaPollerMock : ITaboolaPoller
    {
        public HttpClient Client => throw new System.NotImplementedException();

        public string ServiceName => throw new System.NotImplementedException();

        public Task<string> GetOAuth2Response()
        {
            throw new System.NotImplementedException();
        }

        public async Task<TopCampaignReport> GetTopCampaignReport()
        {
            using (var stream = File.Create(@"mock.json"))
            {
                return await Task.Run(() => Json.Deserialize<TopCampaignReport>(stream));
            }
        }
    }
}