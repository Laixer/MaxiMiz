using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Poller.Taboola.Model;

using AccountEntity = Maximiz.Model.Entity.Account;


namespace Poller.Taboola
{

    /// <summary>
    /// Partial poller that contains all our database
    /// read and write operations.
    /// </summary>
    internal partial class TaboolaPoller
    {


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
                        '{AB}',
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

        private async Task<IEnumerable<AccountEntity>> FetchAdvertiserAccounts(CancellationToken token)
        {
            var sql = @"
                SELECT
                    *
	            FROM
                    public.account
                WHERE
                    publisher = 'taboola'::publisher AND
                    (details::json #>> '{partner_types}')::jsonb ? 'advertiser'";

            using (var connection = _provider.ConnectionScope())
            {
                return await connection.QueryAsync<AccountEntity>(new CommandDefinition(sql, cancellationToken: token));
            }
        }

        // TODO: Return account model according to database scheme.
        private Task<IEnumerable<AccountEntity>> FetchAdvertiserAccountsForCache(CancellationToken token)
            => _cache.GetOrCreateAsync("AdvertiserAccounts", async entry =>
        /// <summary>
        /// Gets all publisher accounts from our database.
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns>All publisher accounts</returns>
        private async Task<IEnumerable<AccountEntity>>
            FetchPublisherAccounts(CancellationToken token)
        {
            var sql = @"
                SELECT
                    *
	            FROM
                    public.account
                WHERE
                    publisher = 'taboola'::publisher AND
                    (details::json #>> '{partner_types}')::jsonb ? 'publisher'";

            using (var connection = _provider.ConnectionScope())
            {
                var result = await connection.QueryAsync<AccountEntity>
                    (new CommandDefinition(sql, cancellationToken: token));
                return result;
            }
        }

            {
                entry.SlidingExpiration = TimeSpan.FromDays(1);
                return await FetchAdvertiserAccounts(token);
            });


    }
}
