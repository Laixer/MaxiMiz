using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web;
using System.Text;
using model.response;
using MaxiMiz.Poller.Poller.Abstract;
using MaxiMiz.Poller.Model.Config;
using MaxiMiz.Poller.Helper;

namespace MaxiMiz.Poller.Poller
{
    internal class TaboolaPoller : ITaboolaPoller
    {
        public string ServiceName { get => "Taboola"; }

        public HttpClient Client { get; private set; }

        private OAuth2Config OAuth2Config { get; }

        private readonly NameValueCollection apiConfig = ConfigurationManager.GetSection("TaboolaApi") as NameValueCollection;


        public TaboolaPoller()
        {

            OAuth2Config = new OAuth2Config(apiConfig["OAuth2ClientId"], apiConfig["OAuth2ClientSecret"], apiConfig["OAuth2RefreshToken"], apiConfig["OAuth2AccessToken"], apiConfig["OAuth2GrantType"], apiConfig["OAuth2Username"], apiConfig["OAuth2Password"]);

            Client = new HttpClient
            {
                BaseAddress = new Uri(apiConfig["BaseUrl"]),

            };
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiConfig["OAuth2AccessToken"]);
            Client.DefaultRequestHeaders.Host = new Uri("https://backstage.taboola.com").Host;
        }
        public async Task<TopCampaignReport> GetTopCampaignReport()
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["start_date"] = "2019-06-20";
            query["end_date"] = "2019-06-20";

            string accountId = apiConfig["AccountId"];
            string urlString = $"api/1.0/{accountId}/reports/top-campaign-content/dimensions/item_breakdown?{query.ToString()}";

            using (var request = new HttpRequestMessage(HttpMethod.Get, urlString)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            })
            using (var res = await Client.SendAsync(request))
            {
                return await Json.Deserialize<TopCampaignReport>(res);
            }

        }


        public async Task<string> GetOAuth2Response()
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["client_id"] = OAuth2Config.OAuth2ClientId;
            query["client_secret"] = OAuth2Config.OAuth2ClientSecret;
            query["username"] = OAuth2Config.username;
            query["password"] = OAuth2Config.password;
            query["grant_type"] = OAuth2Config.grantType;

            var urlString = $"oauth/token?{query.ToString()}";

            using (var content = new FormUrlEncodedContent(new Dictionary<string, string>()))
            using (HttpResponseMessage res = await Client.PostAsync(urlString, content))
            {
                return await res.Content.ReadAsStringAsync();
            }
        }

    }
}
