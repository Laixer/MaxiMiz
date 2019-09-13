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
using AdGroup = Maximiz.Model.Entity.AdGroup;


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
        /// <returns>Task</returns>
        private async Task CommitAdItems(IEnumerable<AdItemEntity> aditems,
            CancellationToken token, bool updateStatus = false)
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
        /// Use this to update values for a campaign in our own database. The
        /// campaign MUST have a (nonzero) GUID to it, else we can't push the
        /// changes to the corresponding database row. This value is stored in
        /// the Id property.
        /// </summary>
        /// <param name="campaign">Campaign with valuid GUID</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        private async Task UpdateLocalCampaignAsync(CampaignEntity campaign,
            CancellationToken token)
        {
            if (campaigns == null || campaigns.Count() <= 0) { return; }
            // Throw on invalid GUID.
            ValidateGuid(campaign);

            var sql = @"
                UPDATE public.campaign
	            SET
                    secondary_id = @SecondaryId,
                    name = @Name, 
                    branding_text = @BrandingText, 
                    language_as_text = @LanguageAsText, 
                    initial_cpc = @InitialCpc, 
                    budget = @Budget, 
                    budget_daily = @DailyBudget, 
                    spent = @Spent, 
                    delivery = CAST (@DeliveryText AS delivery), 
                    start_date = COALESCE(@StartDate, CURRENT_TIMESTAMP),     
                    end_date = VALIDATE_TIMESTAMP(@EndDate), 
                    utm = @Utm, 
                    campaign_group = 48389, 
                    note = @Note, 
                    details = CAST (@Details AS json),

                    location_include = @LocationInclude, 
                    location_exclude = @LocationExclude,
                    language = '{AB}'
                WHERE Id = @Id;";

            //foreach (var campaign in campaigns)
            //{
            //    if (!(campaign.InitialCpc < campaign.DailyBudget || campaign.DailyBudget != null)) { }
            //}
            using (var connection = _provider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    sql, campaign, cancellationToken: token));
            }
        }

        /// <summary>
        /// Deletes a local campaign based on the guid.
        /// </summary>
        /// <param name="guid">The GUID to delete</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        private async Task LocalDeleteCampaignAsync(Guid guid, CancellationToken token)
        {
            var sql = $"DELETE FROM public.campaign WHERE id::text = '{guid.ToString()}'";
            using (var connection = _provider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    sql, cancellationToken: token));
            }
        }

        /// <summary>
        /// Push a single campaign to our own database. The database assigns a 
        /// GUID to the campaign we just pushed. This function writes this GUID
        /// to the campaign and returns it.
        /// 
        /// TODO Duplicate code
        /// 
        /// </summary>
        /// <param name="campaign">The core campaign</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Campaign entity with corresponding GUID</returns>
        private async Task<CampaignEntity> CommitCampaignWriteGuid(CampaignEntity campaign, CancellationToken token)
        {
            var sql = @"
                INSERT INTO
	                public.campaign(
                        secondary_id, name, branding_text, 
                        language_as_text, initial_cpc, budget, 
                        budget_daily, spent, delivery, 
                        start_date, end_date, utm, 
                        campaign_group, note, details,

                        location_include, location_exclude, language)
                VALUES
                    (
                        @SecondaryId,
                        @Name,
                        @BrandingText,
                        @LanguageAsText,
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
                ON CONFLICT (secondary_id) DO NOTHING
                RETURNING Id;";

            using (var connection = _provider.ConnectionScope())
            {
                var guid = await connection.ExecuteScalarAsync<Guid>(
                    new CommandDefinition(sql, campaign, cancellationToken: token));

                // TODO This should be done differently
                if (campaign.Id == Guid.Empty || campaign.Id == null)
                {
                    campaign.Id = guid;
                }
                return campaign;
            }
        }

        /// <summary>
        /// This commits our fetched campaigns to our local database.
        /// </summary>
        /// <param name="campaigns">Core campaign list</param>
        /// <param name="token"></param>
        /// <returns>Task</returns>
        private async Task CommitCampaigns(IEnumerable<CampaignEntity> campaigns,
            CancellationToken token)
        {
            if (campaigns == null || campaigns.Count() <= 0) { return; }

            var sql = @"
                INSERT INTO
	                public.campaign(
                        secondary_id, name, branding_text, 
                        language_as_text, initial_cpc, budget, 
                        budget_daily, spent, delivery, 
                        start_date, end_date, utm, 
                        campaign_group, note, details,

                        location_include, location_exclude, language)
                VALUES
                    (
                        @SecondaryId,
                        @Name,
                        @BrandingText,
                        @LanguageAsText,
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
                ON CONFLICT (secondary_id) DO NOTHING;";

            using (var connection = _provider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    sql, campaigns, cancellationToken: token));
            }
        }

    }

}

