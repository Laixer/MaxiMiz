using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web;
using System.Linq;
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

        private static OAuthHttpClient _client;
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

        /// <summary>
        /// Create or reuse an OAuthHttpClient.
        /// </summary>
        private OAuthHttpClient BuildHttpClient(bool newInstance = false)
        {
            if (_client != null && !newInstance)
            {
                return _client;
            }

            return _client = new OAuthHttpClient
            {
                BaseAddress = new Uri(options.BaseUrl),
                TokenUri = "oauth/token",
                RefreshUri = "oauth/refresh",
            };
        }

        /// <summary>
        /// Gets the Top Campaign Reports for a specific date as specified
        /// in the Backstage documentation, deserializes them and  inserts them into the database
        /// </summary>
        public async override Task<TopCampaignReport> GetTopCampaignReportAsync()
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["start_date"] = DateTime.Now.ToString("yyyy-MM-dd");
            query["end_date"] = DateTime.Now.ToString("yyyy-MM-dd");

            var url = $"api/1.0/{options.AccountId}/reports/top-campaign-content/dimensions/item_breakdown?{query}";

            var result = await RemoteQueryAsync<TopCampaignReport>(HttpMethod.Get, url);
            if (result.Items.Count() <= 0)
            {
                return null;
            }

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

            return null;
        }

        public async Task GetAllCampaigns()
        {
            var url = $"api/1.0/{options.AccountId}/campaigns";

            var result = await RemoteQueryAsync<TopCampaignReport>(HttpMethod.Get, url);
        }

        public async Task GetCampaign()
        {
            var campaign = "2404044";
            var url = $"api/1.0/{options.AccountId}/campaigns/{campaign}";

            var result = await RemoteQueryAsync<TopCampaignReport>(HttpMethod.Get, url);
        }

        public async Task CreateCampaign()
        {
            var url = $"api/1.0/{options.AccountId}/campaigns/";

            var result = await RemoteQueryAsync<TopCampaignReport>(HttpMethod.Post, url);
        }

        private async Task<OAuth2Response> AuthenticateWithPasswordAsync()
        {
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "oauth/token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"client_id", options.OAuth2.ClientId},
                    {"client_secret", options.OAuth2.ClientSecret},
                    {"username", options.OAuth2.Username},
                    {"password", options.OAuth2.Password},
                    {"grant_type", options.OAuth2.GrantType},
                })
            })
            using (var httpResponse = await SendAsync(httpRequest))
            {
                return await Json.DeserializeAsync<OAuth2Response>(httpResponse);
            }
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage message)
        {
            try
            {
                var response = await BuildHttpClient().SendAsync(message).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (HttpRequestException e)
            {
                Logger.LogError($"{message.Method} request to {message.RequestUri} had non {HttpStatusCode.OK} status code.{Environment.NewLine}{e.StackTrace}");
                throw e;
            }
        }

        private async Task<TResult> RemoteQueryAsync<TResult>(HttpMethod method, string url)
            where TResult : class
        {
            using (var httpResult = await SendWithAuthAsync(new HttpRequestMessage(method, url)))
            {
                return await Json.DeserializeAsync<TResult>(httpResult);
            }
        }

        private async Task<HttpResponseMessage> SendWithAuthAsync(HttpRequestMessage message)
        {
            if (OAuth2Session == null)
            {
                OAuth2Session = await AuthenticateWithPasswordAsync();
            }
            else if (IsTokenExpired)
            {
                // OAuth2Session = await RefreshAccessTokenAsync();
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

                _client?.Dispose();
                _client = null;

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
