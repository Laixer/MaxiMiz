using System;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using Microsoft.Extensions.Logging;
using Poller.Model;
using Poller.Model.Response;
using Poller.OAuth;
using Poller.Poller;

namespace Poller.Taboola
{
    internal class TaboolaPoller : IPollerRefreshAdvertisementData, IPollerDataSyncback, IPollerCreateOrUpdateObjects
    {
        private readonly ILogger _logger;
        private readonly DbConnection _connection;
        private readonly HttpManager _client;
        private readonly TaboolaPollerOptions _options;

        public TaboolaPoller(ILogger logger, TaboolaPollerOptions options, DbConnection connection)
        {
            _logger = logger;
            _options = options;
            _connection = connection;

            _client = new HttpManager(_options.BaseUrl)
            {
                TokenUri = "oauth/token",
                RefreshUri = "oauth/token",
                AuthorizationProvider = new OAuthAuthorizationProvider
                {
                    ClientId = _options.OAuth2.ClientId,
                    ClientSecret = _options.OAuth2.ClientSecret,
                    Username = _options.OAuth2.Username,
                    Password = _options.OAuth2.Password,
                }
            };
        }

        /// <summary>
        /// Run the remote query and catch all exceptions where before letting
        /// them propagate upwards.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Object of TResult.</returns>
        protected async Task<TResult> RemoteQueryAndLogAsync<TResult>(HttpMethod method, string url, CancellationToken cancellationToken = default) // TODO: remove default
            where TResult : class
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                _logger.LogTrace($"Executing {method} {url}");

                return await _client.RemoteQueryAsync<TResult>(method, url, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError($"{url}: {e.Message}");
                throw e;
            }
        }

        /// <summary>
        /// Gets the Top Campaign Reports for a specific date as specified
        /// in the Backstage documentation, deserializes them and  inserts them into the database
        /// </summary>
        private async Task GetTopCampaignReportAsync(string account, CancellationToken token)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["start_date"] = query["end_date"] = DateTime.Now.ToString("yyyy-MM-dd");

            var url = $"api/1.0/{account}/reports/top-campaign-content/dimensions/item_breakdown?{query}";

            var result = await RemoteQueryAndLogAsync<TopCampaignReport>(HttpMethod.Get, url, token);
            if (result == null || result.RecordCount <= 0) { return; }

            try
            {
                var sql = @"
                    INSERT INTO
	                    public.item(ad_group, campaign, clicks, impressions, spent, currency, publisher_id, content_url, url)
                    VALUES
                        (
                            1,
                            @Campaign,
                            @Clicks,
                            @Impressions,
                            @Spent,
                            @Currency,
                            @PublisherItemId,
                            @ContentUrl,
                            @Url
                        )
                        ON CONFLICT (publisher_id) DO
                        UPDATE
                        SET
                            clicks = excluded.clicks, impressions = excluded.impressions, spent = excluded.spent";

                await _connection.OpenAsync();
                await _connection.ExecuteAsync(new CommandDefinition(sql, result.Items, cancellationToken: token));
            }
            finally
            {
                // TODO: This should not be required
                _connection.Close();
            }
        }

        private async Task GetAllAccounts(CancellationToken token)
        {
            var url = $"api/1.0/users/current/allowed-accounts/";

            var accounts = await RemoteQueryAndLogAsync<AllowedAccounts>(HttpMethod.Get, url, token);
            if (accounts == null || accounts.Items.Count() <= 0) { return; }

            try
            {
                await _connection.OpenAsync();
                await _connection.ExecuteAsync(
                    @"INSERT INTO public.account_integration(publisher_id, name, type, currency, account)
                          VALUES (@Id, @Name, @Type, @Currency, @AccountId)", accounts.Items);
            }
            finally
            {
                // TODO: This should not be required
                _connection.Close();
            }
        }

        private async Task GetAllCampaigns(string account, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns";

            var result = await RemoteQueryAndLogAsync<Campaign>(HttpMethod.Get, url, token);
        }

        private async Task GetCampaign(string account, string campaign, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}";

            var result = await RemoteQueryAndLogAsync<Campaign>(HttpMethod.Get, url, token);
        }

        private async Task CreateCampaign(string account, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/";

            await RemoteQueryAndLogAsync<TopCampaignReport>(HttpMethod.Post, url, token);
        }

        private async Task UpdateCampaignStatus(string account, string campaign, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}";

            var result = await RemoteQueryAndLogAsync<TopCampaignReport>(HttpMethod.Put, url, token);
        }

        private async Task DeleteCampaign(string account, string campaign, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}";

            var result = await RemoteQueryAndLogAsync<TopCampaignReport>(HttpMethod.Delete, url, token);
        }

        private async Task GetCampaignAllItems(string account, string campaign, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}/items";

            var result = await RemoteQueryAndLogAsync<Campaign>(HttpMethod.Get, url, token);
        }

        private async Task GetCampaignItem(string account, string campaign, string item, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}/items/{item}";

            var result = await RemoteQueryAndLogAsync<Campaign>(HttpMethod.Get, url, token);
        }

        public async Task RefreshAdvertisementDataAsync(PollerContext context, CancellationToken token)
        {
            await GetTopCampaignReportAsync("socialentertainment-network", token);
        }

        public async Task DataSyncbackAsync(CancellationToken token)
        {
            await GetAllAccounts(token);
        }

        public Task CreateOrUpdateObjectsAsync(CancellationToken token)
        {
            //

            return Task.CompletedTask;
        }
    }
}
