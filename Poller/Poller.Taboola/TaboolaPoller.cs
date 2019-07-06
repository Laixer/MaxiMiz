using System;
using System.Net;
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
using System.Data.Common;
using Dapper;

namespace Poller.Taboola
{
    [Publisher("Taboola")]
    public class TaboolaPoller : RemotePublisher, IDisposable
    {
        private static readonly string TokenType = "Bearer";

        private HttpClient client;
        private OAuth2Response OAuth2Session;
        private readonly DbConnection connection;
        private readonly TaboolaPollerOptions options;

        /// <summary>
        /// Creates a TaboolaPoller for fetching Data from Taboola.
        /// </summary>
        /// <param name="logger">A logger for this poller.</typeparam>
        /// <param name="options">An instance of options required for requests.</param>
        /// <param name="connection">The database connections to use for inserting fetched data.</param>
        public TaboolaPoller(ILogger<TaboolaPoller> logger, IOptions<TaboolaPollerOptions> options, DbConnection connection)
            : base(logger)
        {
            this.options = options?.Value;
            this.connection = connection;
        }

        private bool IsTokenExpired
        {
            get
            {
                // TODO: Implement logic
                return false;
            }
        }

        private HttpClient BuildHttpClient(bool newInstance = false)
        {
            if (client != null && !newInstance)
            {
                return client;
            }

            client = new HttpClient
            {
                BaseAddress = new Uri(options.BaseUrl),
            };

            client.DefaultRequestHeaders.Host = new Uri(options.BaseUrl).Host;
            return client;
        }

        private Task RefreshAccessToken()
        {
            // TODO: Implement this as per the manual from backstage
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the Top Campaign Reports for a specific date as specified
        /// in the Backstage documentation, deserializes them and  inserts them into the database
        /// </summary>
        public async override Task<TopCampaignReport> GetTopCampaignReportAsync()
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["start_date"] = "2019-07-06";
            query["end_date"] = "2019-07-06";

            string urlString = $"api/1.0/{options.AccountId}/reports/top-campaign-content/dimensions/item_breakdown?{query.ToString()}";
            using (var req = new HttpRequestMessage(HttpMethod.Get, urlString)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            })
            using (HttpResponseMessage res = await SendWithAuthAsync(req))
            {
                var result = await Json.DeserializeAsync<TopCampaignReport>(res);
                try
                {
                    await connection.OpenAsync();
                    await connection.ExecuteAsync(
                        @"INSERT INTO public.item(ad_group, campaign, clicks, impressions, spent, currency, publisher_id, content_url, url)
                          VALUES (1, @Campaign, @Clicks, @Impressions, @Spent, @Currency, @PublisherItemId, @ContentUrl, @Url)", result.Items);
                }
                finally
                {
                    // TODO: This should not be required
                    connection.Close();
                }
            }
            return null;
        }

        private async Task AuthenticateWithPasswordAsync()
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["client_id"] = options.OAuth2.ClientId;
            query["client_secret"] = options.OAuth2.ClientSecret;
            query["username"] = options.OAuth2.Username;
            query["password"] = options.OAuth2.Password;
            query["grant_type"] = options.OAuth2.GrantType;
            var urlString = $"oauth/token?{query.ToString()}";

            using (var req = new HttpRequestMessage(HttpMethod.Post, urlString)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>())
            })
            using (var res = await SendAsync(req))
            {
                OAuth2Session = await Json.DeserializeAsync<OAuth2Response>(res);
            }
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage message)
        {
            try
            {
                var response = await BuildHttpClient().SendAsync(message);
                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (HttpRequestException hre)
            {
                Logger.LogError($"{message.Method} request to {message.RequestUri} had non {HttpStatusCode.OK} status code.{Environment.NewLine}{hre.StackTrace}");
                throw hre;
            }
        }

        private async Task<HttpResponseMessage> SendWithAuthAsync(HttpRequestMessage message)
        {
            if (OAuth2Session == null)
            {
                await AuthenticateWithPasswordAsync();
            }
            else if (IsTokenExpired)
            {
                await RefreshAccessToken();
            }

            message.Headers.Authorization = new AuthenticationHeaderValue(TokenType, OAuth2Session.AccessToken);

            return await SendAsync(message);
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
