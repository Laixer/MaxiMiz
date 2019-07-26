using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Poller.Database;
using Poller.Extensions;
using Poller.Helper;
using Poller.OAuth;
using Poller.Poller;
using Poller.Taboola.Model;

namespace Poller.Taboola
{
    internal class TaboolaPoller : IPollerRefreshAdvertisementData, IPollerDataSyncback, IPollerCreateOrUpdateObjects, IDisposable
    {
        private readonly ILogger _logger;
        private readonly DbProvider _provider;
        private readonly IMemoryCache _cache;
        private readonly HttpManager _client;

        public TaboolaPoller(ILogger logger, TaboolaPollerOptions options, DbProvider provider, IMemoryCache cache)
        {
            _logger = logger;
            _provider = provider;
            _cache = cache;

            _client = new HttpManager(options.BaseUrl)
            {
                TokenUri = "oauth/token",
                RefreshUri = "oauth/token",
                AuthorizationProvider = new OAuthAuthorizationProvider
                {
                    ClientId = options.OAuth2.ClientId,
                    ClientSecret = options.OAuth2.ClientSecret,
                    Username = options.OAuth2.Username,
                    Password = options.OAuth2.Password,
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
            catch (Exception e) when (e as OperationCanceledException == null && e as TaskCanceledException == null)
            {
                _logger.LogError($"{url}: {e.Message}");
                throw e;
            }
        }

        /// <summary>
        /// Gets the Top Campaign Reports for a specific date as specified
        /// in the Backstage documentation, deserializes them and  inserts them into the database
        /// </summary>
        private Task<EntityList<AdItem>> GetTopCampaignReportAsync(string account, CancellationToken token)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["end_date"] = query["start_date"] = DateTime.Now.ToString("yyyy-MM-dd");

            var url = $"api/1.0/{account}/reports/top-campaign-content/dimensions/item_breakdown?{query}";

            return RemoteQueryAndLogAsync<EntityList<AdItem>>(HttpMethod.Get, url, token);
        }

        private Task<EntityList<Account>> GetAllAccounts(CancellationToken token)
        {
            var url = $"api/1.0/users/current/allowed-accounts/";

            return RemoteQueryAndLogAsync<EntityList<Account>>(HttpMethod.Get, url, token);
        }

        private Task<EntityList<Campaign>> GetAllCampaigns(string account, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns";

            return RemoteQueryAndLogAsync<EntityList<Campaign>>(HttpMethod.Get, url, token);
        }

        private async Task GetCampaign(string account, string campaign, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}";

            var result = await RemoteQueryAndLogAsync<Campaign>(HttpMethod.Get, url, token);
        }

        private Task<Campaign> CreateCampaign(string account, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/";

            return RemoteQueryAndLogAsync<Campaign>(HttpMethod.Post, url, token);
        }

        private async Task UpdateCampaignStatus(string account, string campaign, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}";

            var result = await RemoteQueryAndLogAsync<Campaign>(HttpMethod.Put, url, token);
        }

        private Task<Campaign> DeleteCampaign(string account, string campaign, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}";

            return RemoteQueryAndLogAsync<Campaign>(HttpMethod.Delete, url, token);
        }

        private Task<EntityList<AdItem>> GetCampaignAllItems(string account, string campaign, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}/items";

            return RemoteQueryAndLogAsync<EntityList<AdItem>>(HttpMethod.Get, url, token);
        }

        private Task<AdItem> GetCampaignItem(string account, string campaign, string item, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}/items/{item}";

            return RemoteQueryAndLogAsync<AdItem>(HttpMethod.Get, url, token);
        }

        // TODO: ad_group
        private async Task CommitCampaignItems(EntityList<AdItem> aditems, CancellationToken token, bool updateStatus = false)
        {
            if (aditems == null || aditems.Items.Count() <= 0) { return; }

            var sql = @"
                INSERT INTO
	                public.ad_item AS INCLUDED (secondary_id, ad_group, title, url, content, cpc, spent, clicks, impressions, actions, details, status, approval_state)
                VALUES
                    (
                        @Id,
                        2,
                        LEFT(@TitleText, 128),
                        @Url,
                        @Content,
                        @Cpc,
                        @Spent,
                        @Clicks,
                        @Impressions,
                        @Actions,
                        @Details::json,
                        CAST (@StatusText AS ad_item_status),
                        CAST (@ApprovalStateText AS approval_state)
                    )
                ON CONFLICT (secondary_id) DO UPDATE
                SET
                    title = EXCLUDED.title,
                    cpc = GREATEST(INCLUDED.cpc, EXCLUDED.cpc),
                    spent = GREATEST(INCLUDED.spent, EXCLUDED.spent),
                    clicks = GREATEST(INCLUDED.clicks, EXCLUDED.clicks),
                    impressions = GREATEST(INCLUDED.impressions, EXCLUDED.impressions),
                    actions = GREATEST(INCLUDED.actions, EXCLUDED.actions),
                    details = COALESCE(INCLUDED.details, EXCLUDED.details)";

            if (updateStatus)
            {
                sql += @",
                    status = EXCLUDED.status,
                    approval_state = EXCLUDED.approval_state";
            }

            using (var connection = _provider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(sql, aditems.Items, cancellationToken: token));
            }
        }

        private async Task CommitAccounts(EntityList<Account> accounts, CancellationToken token)
        {
            if (accounts == null || accounts.Items.Count() <= 0) { return; }

            var sql = @"
                INSERT INTO
	                public.account(secondary_id, publisher, name, currency, details)
                VALUES
                    (
                        @Id,
                        'taboola',
                        @AccountId,
                        @Currency,
                        @Details::json
                    )
                ON CONFLICT (name) DO NOTHING";

            using (var connection = _provider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(sql, accounts.Items, cancellationToken: token));
            }
        }

        //TODO:
        // - campaign_group
        // - location_include
        // - location_exclude
        // - language
        private async Task CommitCampaigns(EntityList<Campaign> campaigns, CancellationToken token)
        {
            if (campaigns == null || campaigns.Items.Count() <= 0) { return; }

            foreach (var item in campaigns.Items)
            {
                if (item.Cpc > item.DailyCap)
                {
                    item.DailyCap = null;
                }
                if (item.EndDate.HasValue)
                {
                    if (item.EndDate.Value.Year == 9999 && item.EndDate.Value.Month == 12 && item.EndDate.Value.Day == 31)
                    {
                        item.EndDate = null;
                    }
                }
            }

            var sql = @"
                INSERT INTO
	                public.campaign(secondary_id, name, branding_text, location_include, location_exclude, language, initial_cpc, budget, budget_daily, budget_model, delivery, start_date, end_date, utm, campaign_group, note)
                VALUES
                    (
                        @Id,
                        @Name,
                        @Branding,
                        '{0}',
                        '{0}',
                        '{XYZ}',
                        @Cpc,
                        @SpendingLimit,
                        @DailyCap,
                        CAST (@SpendingLimitModelText AS budget_model),
                        CAST (@DeliveryText AS delivery),
                        COALESCE(@StartDate, CURRENT_TIMESTAMP),
                        @EndDate,
                        @Utm,
                        48389,
                        @Note
                    )
                ON CONFLICT (secondary_id) DO NOTHING";

            using (var connection = _provider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(sql, campaigns.Items, cancellationToken: token));
            }
        }

        private async Task<IEnumerable<Account>> FetchAdvertiserAccounts(CancellationToken token)
        {
            var sql = @"
                SELECT
                    publisher, name
	            FROM
                    public.account
                WHERE
                    publisher = 'taboola'::publisher AND
                    (details::json #>> '{partner_types}')::jsonb ? 'advertiser'";

            using (var connection = _provider.ConnectionScope())
            {
                return await connection.QueryAsync<Account>(new CommandDefinition(sql, cancellationToken: token));
            }
        }

        // TODO: Return account model according to database scheme.
        private Task<IEnumerable<Account>> FetchAdvertiserAccountsForCache(CancellationToken token)
            => _cache.GetOrCreateAsync("AdvertiserAccounts", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(1);
                return await FetchAdvertiserAccounts(token);
            });

        public async Task RefreshAdvertisementDataAsync(PollerContext context, CancellationToken token)
        {
            var accounts = await FetchAdvertiserAccountsForCache(token);
            foreach (var account in accounts.ToList().Shuffle())
            {
                var result = await GetTopCampaignReportAsync(account.Name, token);
                await CommitCampaignItems(result, token);

                // Prevent spamming.
                await Task.Delay(250, token);
            }
        }

        public async Task DataSyncbackAsync(PollerContext context, CancellationToken token)
        {
            // Accounts almost never change.
            if (context.RunCount + 1 % 4 == 0)
            {
                _logger.LogInformation("Syncback account information");

                var result = await GetAllAccounts(token);
                await CommitAccounts(result, token);
            }

            var accounts = await FetchAdvertiserAccountsForCache(token);
            foreach (var account in accounts.ToList().Shuffle().Take(2))
            {
                var result = await GetAllCampaigns(account.Name, token);
                await CommitCampaigns(result, token);

                // Reorder the items with the idea of risk spreading.
                foreach (var item in result.Items.ToList().Shuffle().Take(100))
                {
                    try
                    {
                        var result2 = await GetCampaignAllItems(account.Name, item.Id, token);
                        await CommitCampaignItems(result2, token, true);

                        // Prevent spamming.
                        await Task.Delay(250, token);

                        context.MarkProgress(token);
                    }
                    catch (TaskCanceledException)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                }
            }
        }

        public Task CreateOrUpdateObjectsAsync(PollerContext context, CancellationToken token)
        {
            //

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
