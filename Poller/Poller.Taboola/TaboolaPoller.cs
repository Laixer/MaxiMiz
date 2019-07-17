using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Linq;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Poller.OAuth;
using Poller.Publisher;
using Poller.Model;
using Poller.Model.Response;
using Dapper;

namespace Poller.Taboola
{
    [Publisher("Taboola")]
    public class TaboolaPoller : RemotePublisher, IDisposable
    {
        private readonly Lazy<HttpManager> _client;
        private readonly DbConnection connection;
        private readonly TaboolaPollerOptions options;

        protected HttpManager HttpManager { get => _client.Value; }

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

            // Lazy initialization prevent performance hit on process start.
            _client = new Lazy<HttpManager>(() =>
            {
                return new HttpManager(this.options.BaseUrl)
                {
                    TokenUri = "oauth/token",
                    RefreshUri = "oauth/token",
                    AuthorizationProvider = new OAuthAuthorizationProvider
                    {
                        ClientId = this.options.OAuth2.ClientId,
                        ClientSecret = this.options.OAuth2.ClientSecret,
                        Username = this.options.OAuth2.Username,
                        Password = this.options.OAuth2.Password,
                    }
                };
            });
        }

        /// <summary>
        /// Run the remote query and catch all exceptions where before letting
        /// them propagate upwards.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <returns>Object of TResult.</returns>
        protected async Task<TResult> RemoteQueryAndLogAsync<TResult>(HttpMethod method, string url, CancellationToken cancellationToken = default)
            where TResult : class
        {
            try
            {
                Logger.LogTrace($"Executing {method} {url}");

                return await HttpManager.RemoteQueryAsync<TResult>(method, url);
            }
            catch (Exception e)
            {
                Logger.LogError($"{url}: {e.Message}");
                throw e;
            }
        }

        public async Task GetAllAccounts()
        {
            var url = $"api/1.0/users/current/allowed-accounts/";

            var accounts = await RemoteQueryAndLogAsync<AllowedAccounts>(HttpMethod.Get, url);
            if (accounts == null || accounts.Items.Count() <= 0)
            {
                return;
            }

            try
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(
                    @"INSERT INTO public.account_integration(publisher_id, name, type, currency, account)
                          VALUES (@Id, @Name, @Type, @Currency, @AccountId)", accounts.Items);
            }
            finally
            {
                // TODO: This should not be required
                connection.Close();
            }
        }

        /// <summary>
        /// Gets the Top Campaign Reports for a specific date as specified
        /// in the Backstage documentation, deserializes them and  inserts them into the database
        /// </summary>
        public async override Task GetTopCampaignReportAsync()
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["start_date"] = query["end_date"] = DateTime.Now.ToString("yyyy-MM-dd");

            // TODO: use fetched accountId
            var url = $"api/1.0/{options/*.AccountId*/}/reports/top-campaign-content/dimensions/item_breakdown?{query}";

            var result = await RemoteQueryAndLogAsync<TopCampaignReport>(HttpMethod.Get, url);
            if (result == null || result.RecordCount <= 0)
            {
                return;
            }

            try
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(
                    @"INSERT INTO public.item(ad_group, campaign, clicks, impressions, spent, currency, publisher_id, content_url, url) 
                      VALUES (1, 
                        @Campaign, 
                        @Clicks, 
                        @Impressions, 
                        @Spent, 
                        @Currency, 
                        @PublisherItemId, 
                        @ContentUrl, 
                        @Url) ON CONFLICT (publisher_id) DO
                      UPDATE 
                      SET clicks = @Clicks, 
                          impressions = @Impressions, 
                          spent = @Spent",
                        result.Items);
            }
            finally
            {
                // TODO: This should not be required
                connection.Close();
            }
        }

        public async Task GetAllCampaigns()
        {
            //TODO use fetched accountId
            var url = $"api/1.0/{options/*.AccountId*/}/campaigns";

            var campaigns = await RemoteQueryAndLogAsync<Campaign>(HttpMethod.Get, url);
        }

        public async Task GetCampaign()
        {
            var campaign = "2162154";
            //TODO use fetched accountId
            var url = $"api/1.0/{options/*.AccountId*/}/campaigns/{campaign}";

            var campaigns = await RemoteQueryAndLogAsync<Campaign>(HttpMethod.Get, url);
        }

        public async Task CreateCampaign()
        {
            //TODO use fetched accountId
            var url = $"api/1.0/{options/*.AccountId*/}/campaigns/";

            await RemoteQueryAndLogAsync<TopCampaignReport>(HttpMethod.Post, url);
        }

        public void Dispose()
        {
            _client?.Value?.Dispose();
        }
    }
}
