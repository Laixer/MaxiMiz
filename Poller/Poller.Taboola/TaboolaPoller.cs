using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using MaxiMiz.Poller.Model.Response;

using Poller.Publisher;
using Poller.Helper;

namespace Poller.Taboola
{
    [Publisher("Taboola")]
    public class TaboolaPoller : RemotePublisher, IDisposable
    {
        private HttpClient client;

        private readonly TaboolaPollerOptions options;

        public TaboolaPoller(ILogger<TaboolaPoller> logger, IOptions<TaboolaPollerOptions> options)
            : base(logger)
        {
            this.options = options?.Value;
        }

        private HttpClient BuildHttpClient(bool newInstane = false)
        {
            if (client != null && !newInstane)
            {
                return client;
            }

            client = new HttpClient
            {
                BaseAddress = new Uri(options.BaseUrl),
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sometoken"); // TODO: set access token
            client.DefaultRequestHeaders.Host = new Uri("https://backstage.taboola.com").Host;

            return client;
        }

        public async override Task<TopCampaignReport> GetTopCampaignReportAsync()
        {
            Logger.LogInformation("Hi there");

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["start_date"] = "2019-06-20";
            query["end_date"] = "2019-06-20";

            string urlString = $"api/1.0/{options.AccountId}/reports/top-campaign-content/dimensions/item_breakdown?{query.ToString()}";

            using (var request = new HttpRequestMessage(HttpMethod.Get, urlString)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            })
            using (var res = await BuildHttpClient().SendAsync(request))
            {
                return await Json.DeserializeAsync<TopCampaignReport>(res);
            }
        }

        public async Task<string> GetOAuth2ResponseAsync()
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["client_id"] = options.OAuth2.ClientId;
            query["client_secret"] = options.OAuth2.ClientSecret;
            query["username"] = options.OAuth2.Username;
            query["password"] = options.OAuth2.Password;
            query["grant_type"] = ""; // TODO: Set some grand type

            var urlString = $"oauth/token?{query.ToString()}";

            using (var content = new FormUrlEncodedContent(new Dictionary<string, string>()))
            using (var res = await BuildHttpClient().PostAsync(urlString, content))
            {
                return await res.Content.ReadAsStringAsync();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                client?.Dispose();
                client = null;

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TaboolaPoller()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
