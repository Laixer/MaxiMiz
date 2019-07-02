using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;
using model.response;

namespace poller
{
    internal class TaboolaPoller : IRestPoller
    {
        public string ServiceName { get => "Taboola"; }

        public HttpClient Client { get; private set; }

        public TaboolaPoller()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri("")
            };
            // GetOAuth2Response();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            // client.DefaultRequestHeaders.Add("Host", new "https://backstage.taboola.com");
            


        }

        string GetToken()
        {
            return null;
        }

        public async Task<string> GetTopCampaignReport()
        {
            using (HttpRequestMessage request = new HttpRequestMessage(
                            HttpMethod.Get,
                        "/backstage/api/1.0/[accountId]/reports/top-campaign-content/dimensions/item_breakdown?start_date=2019-06-20&end_date=2019-06-28"))
            {
                using (HttpResponseMessage res = await Client.SendAsync(request))
                {
                    return await res.Content.ReadAsStringAsync();
                }
            }
        }

        public async Task<OAuth2Response> GetOAuth2Response()
        {
            using (HttpResponseMessage res = await Client.PostAsync("oauth/token?client-id=&client_secret=&username=&password=&grant_type=password", null))
            using (Stream responseStream = await res.Content.ReadAsStreamAsync())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(OAuth2Response));
                return (OAuth2Response)ser.ReadObject(responseStream);

            }
        }




    }
}
