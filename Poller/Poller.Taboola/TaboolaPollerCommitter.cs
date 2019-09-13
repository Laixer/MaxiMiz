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
    /// Partial poller that contains all our database write operations.
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
        /// Commits a single ad item.
        /// </summary>
        /// <param name="adItem">The ad item to commit</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        private async Task CommitAdItem(AdItemEntity adItem,
            CancellationToken token)
        {
            var list = new List<AdItemEntity>();
            list.Add(adItem);
            await CommitAdItems(list, token);
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

            //foreach (var campaign in campaigns)
            //{
            //    if (!(campaign.InitialCpc < campaign.DailyBudget || campaign.DailyBudget != null)) { }
            //}

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
                        VALIDATE_TIMESTAMP(@EndDate),
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
                await connection.ExecuteAsync(new CommandDefinition(
                    sql, campaigns, cancellationToken: token));
            }
        }

        /// <summary>
        /// </summary>
        {
            var sql = @"



            using (var connection = _provider.ConnectionScope())
            {
            }
        }

    }
}
