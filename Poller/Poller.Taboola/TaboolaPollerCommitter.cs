using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Poller.Taboola.Model;
using Poller.Taboola.Mapper;

using AccountEntity = Maximiz.Model.Entity.Account;
using AdItemEntity = Maximiz.Model.Entity.AdItem;
using CampaignEntity = Maximiz.Model.Entity.Campaign;


namespace Poller.Taboola
{

    /// <summary>
    /// Partial poller that contains all our database
    /// read and write operations.
    /// </summary>
    internal partial class TaboolaPoller
    {

        /// <summary>
        /// Commits our accounts to the database.
        /// </summary>
        /// <param name="accounts">The core accounts</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Nothing (task)</returns>
        private async Task CommitAccounts(
            IEnumerable<AccountEntity> accounts,
            CancellationToken token)
        {
            if (accounts == null || accounts.Count() <= 0) { return; }
            // TODO Validate format? Or assume format? I think we should validate here.

            // TODO char name 265 should be 256? Don't care I guess? Or optimized because 2^8?
            var sql = @"
                INSERT INTO
	                public.account(secondary_id, publisher, name, currency, details)
                VALUES
                    (
                        @SecondaryId,
                        CAST (@PublisherText AS publisher),
                        LEFT(@Name, 265),
                        @Currency,
                        CAST (@Details AS json)
                    )
                ON CONFLICT (name) DO NOTHING";

            using (var connection = _provider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    sql, accounts, cancellationToken: token));
            }
        }

        /// <summary>
        /// Commits a list of campaign items to our database.
        /// These entries have already been converted.
        /// TODO Bulk insert.
        /// TODO ad_group
        /// </summary>
        /// <param name="aditems">The list of ad items</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="updateStatus">If we want to update
        /// the items status and approval state</param>
        /// <returns>Nothing (task)</returns>
        private async Task CommitCampaignItems(
            IEnumerable<AdItemEntity> aditems,
            CancellationToken token,
            bool updateStatus = false)
        {
            if (aditems == null || aditems.Count() <= 0) { return; }

            var sql = @"
                INSERT INTO
	                public.ad_item AS INCLUDED (secondary_id, ad_group, title, url, content, cpc, spent, clicks, impressions, actions, details, status, approval_state)
                VALUES
                    (
                        @SecondaryId,
                        2,
                        LEFT(@Title, 128),
                        @Url,
                        @Content,
                        @Cpc,
                        @Spent,
                        @Clicks,
                        @Impressions,
                        @Actions,
                        CAST (@Details AS json),
                        CAST (@StatusText AS ad_item_status),
                        CAST (@ApprovalStateText AS approval_state)
                    )
                ON CONFLICT (secondary_id) DO UPDATE
                SET
                    cpc = GREATEST(INCLUDED.cpc, EXCLUDED.cpc),
                    spent = GREATEST(INCLUDED.spent, EXCLUDED.spent),
                    clicks = GREATEST(INCLUDED.clicks, EXCLUDED.clicks),
                    impressions = GREATEST(INCLUDED.impressions, EXCLUDED.impressions),
                    actions = GREATEST(INCLUDED.actions, EXCLUDED.actions),
                    details = COALESCE(INCLUDED.details, EXCLUDED.details)";

            // If we update the items status we also
            // include the status and approval state.
            if (updateStatus)
            {
                sql += @",
                    status = EXCLUDED.status,
                    approval_state = EXCLUDED.approval_state";
            }

            // Call for execution
            using (var connection = _provider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    sql, aditems, cancellationToken: token));
            }
        }

        /// <summary>
        /// This commits our fetched campaigns to our local
        /// database.
        /// </summary>
        /// <param name="campaigns">Core campaign list</param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task CommitCampaigns(
            IEnumerable<CampaignEntity> campaigns,
            CancellationToken token)
        {
            if (campaigns == null || campaigns.Count() <= 0) { return; }



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
	                public.campaign(
                        secondary_id, name, branding_text, 
                        language_as_text, initial_cpc, budget, 
                        budget_daily, spent, delivery, 
                        start_date, end_date, utm, 
                        campaign_group, note, details,

                        location_include, location_exclude,
                        language)
                VALUES
                    (
                        @SecondaryId,
                        @Name,
                        @BrandingText,
                        @Language,
                        @InitialCpc,
                        @Budget,
                        @DailyBudget,
                        @Spent,
                        CAST (@DeliveryText AS delivery),
                        COALESCE(@StartDate, CURRENT_TIMESTAMP),
                        VALIDATE_DATE(@EndDate),
                        @Utm,
                        48389,
                        @Note,
                        CAST (@Details AS json),

                        @LocationInclude,
                        @LocationExclude,
                        '{AB}'
                    )
                ON CONFLICT (secondary_id) DO NOTHING";

            using (var connection = _provider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(sql, campaigns.Items, cancellationToken: token));
            }
        }

        /// <summary>
        /// Gets all advertiser accounts from our database.
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns>All advertiser accounts</returns>
        private async Task<IEnumerable<AccountEntity>>
            FetchAdvertiserAccounts(CancellationToken token)
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
                var result = await connection.QueryAsync<AccountEntity>
                    (new CommandDefinition(sql, cancellationToken: token));
                return result;
            }
        }

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

        /// <summary>
        /// This adds the accounts that are present in
        /// our OWN database to the cache.
        /// TODO: Return account model according to
        /// database scheme.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private Task<IEnumerable<AccountEntity>>
            FetchLocalAdvertiserAccountsForCache
            (CancellationToken token)
        {
            return _cache.GetOrCreateAsync(
                "AdvertiserAccounts", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(1);
                return await FetchAdvertiserAccounts(token);
            });
        }
    }
}
