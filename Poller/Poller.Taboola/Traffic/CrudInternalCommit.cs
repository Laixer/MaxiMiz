using Dapper;
using Maximiz.Model.Entity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Poller.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Poller.Taboola.Traffic
{
    /// <summary>
    /// Responsible for performing update and delete operations in our own 
    /// database. This is a partial class.
    /// </summary>
    internal partial class CrudInternal
    {

        /// <summary>
        /// Logging interface.
        /// </summary>
        private ILogger _logger;

        /// <summary>
        /// The database connection provider object.
        /// </summary>
        private readonly DbProvider _dbProvider;

        /// <summary>
        /// Cache for storing frequently polled data which does noto change often.
        /// </summary>
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Constructor for dependency injection.
        /// TODO Enum arrays non texted --> does this work?
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="dbProvider">Database connection provider object</param>
        /// <param name="cache">Injected cache</param>
        public CrudInternal(ILogger logger, DbProvider dbProvider, IMemoryCache cache)
        {
            _logger = logger;
            _dbProvider = dbProvider;
            _cache = cache;
        }

        /// <summary>
        /// Updates a campaign in our internal database.
        /// </summary>
        /// <param name="campaign">The campaign to update</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        public async Task UpdateCampaignAsync(Campaign campaign, CancellationToken token)
        {
            var sql = @"
                UPDATE public.campaign
	            SET
                    secondary_id = @SecondaryId,
                    name = @Name,
                    branding_text = @BrandingText,
                    utm = @Utm, 
                    initial_cpc = @InitialCpc, 
                    budget_daily = @BudgetDaily, 
                    bid_strategy = CAST (@BidStrategyText AS bid_strategy),
                    budget = @Budget, 
                    budget_model = CAST (@BudgetModelText AS budget_model),
                    note = @Note, 
                    spent = @Spent, 
                    start_date = @StartDate,     
                    end_date = VALIDATE_TIMESTAMP(@EndDate), 
                    approval_state = CAST (@ApprovalStateText AS approval_state),
                    status = CAST (@StatusText AS campaign_status),
                    delivery = CAST (@DeliveryText AS delivery), 

                    details = CAST (@Details AS json),

                    location_include = @LocationInclude, 
                    location_exclude = @LocationExclude,
                    devices = @Devices,
                    operating_systems = @OperatingSystems, 
                    connection_types = @ConnectionTypes,                    

                    target_url = @TargetUrl,
                    campaign_group_guid = @CampaignGroupGuid, 
                    publisher = CAST (@PublisherText as publisher),
                    language = @Language
                WHERE Id = @Id;";

            using (var connection = _dbProvider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    sql, campaign, cancellationToken: token));
            }
        }

        /// <summary>
        /// Deletes a campaign from our internal database.
        /// </summary>
        /// <param name="campaign">The campaign to delete</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        public async Task DeleteCampaignAsync(Campaign campaign, CancellationToken token)
        {
            var sql = $"DELETE FROM public.campaign WHERE id::text = '{campaign.Id.ToString()}'";
            using (var connection = _dbProvider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    sql, cancellationToken: token));
            }
        }

        /// <summary>
        /// Updates an ad item in our internal database.
        /// </summary>
        /// <param name="adItem">The ad item to update</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        public async Task UpdateAdItemAsync(AdItem adItem, CancellationToken token)
        {
            var sql = @"
                UPDATE public.ad_item
	            SET
                    secondary_id = @SecondaryId,
                    url = @Url,
                    image_url = @ImageUrl,
                    title = @Title,
                    approval_state = CAST (@ApprovalStateText as approval_state),
                    status = CAST (@StatusText AS ad_item_status),
                    spent = @Spent,
                    clicks = @Clicks,
                    impressions = @Impressions,
                    actions = @Actions,
                    cpc = @Cpc,

                    details = CAST (@Details AS json),

                    content = @Content,
                    campaign_guid = @CampaignGuid,
                    ad_group_guid = @AdGroupGuid,
                    ad_group_image_index = @AdGroupImageIndex,
                    ad_group_title_index = @AdGroupTitleIndex,
                    modified_beyond_ad_group = @ModifiedBeyondAdGroup
                WHERE Id = @Id;";

            try
            {
                using (var connection = _dbProvider.ConnectionScope())
                {
                    await connection.ExecuteAsync(new CommandDefinition(
                        sql, adItem, cancellationToken: token));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Deletes an ad item from our internal database.
        /// </summary>
        /// <param name="adItem">The ad item to delete</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        public async Task DeleteAdItemAsync(AdItem adItem, CancellationToken token)
        {
            var sql = $"DELETE FROM public.ad_item WHERE id::text = '{adItem.Id.ToString()}'";
            using (var connection = _dbProvider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    sql, cancellationToken: token));
            }
        }

        /// <summary>
        /// This commits our fetched campaigns to our local database.
        /// </summary>
        /// <param name="campaigns">Core campaign list</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        public async Task CommitCampaignBulk(IEnumerable<Campaign> campaigns,
            CancellationToken token)
        {
            if (campaigns == null || campaigns.ToList().Count() <= 0) { return; }

            var sql = @"
                INSERT INTO
	                public.campaign(
                        secondary_id, 
                        name, 
                        branding_text, 
                        utm, 
                        initial_cpc, 
                        budget_daily,
                        bid_strategy,
                        budget, 
                        budget_model,
                        note, 
                        spent, 
                        start_date, 
                        end_date,
                        approval_state,
                        status,
                        delivery,

                        details,

                        location_include, 
                        location_exclude, 
                        devices,
                        operating_systems,
                        connection_types,

                        target_url,
                        campaign_group_guid, 
                        publisher,
                        language)
                VALUES
                    (
                        @SecondaryId,
                        @Name,
                        @BrandingText,
                        @Utm,
                        @InitialCpc,
                        @BudgetDaily,
                        CAST (@BidStrategyText AS bid_strategy),
                        @Budget,
                        CAST (@BudgetModelText AS budget_model),
                        @Note,
                        @Spent,
                        @StartDate,
                        VALIDATE_TIMESTAMP(@EndDate),
                        CAST (@ApprovalStateText AS approval_state),
                        CAST (@StatusText AS campaign_status),
                        CAST (@DeliveryText AS delivery),

                        CAST (@Details AS json),

                        @LocationInclude,
                        @LocationExclude,
                        @Devices,
                        @OperatingSystems,
                        @ConnectionTypes,

                        @TargetUrl,
                        @CampaignGroupGuid,
                        CAST (@PublisherText as publisher),
                        @Language
                    )
                ON CONFLICT (secondary_id) DO NOTHING;";

            using (var connection = _dbProvider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    sql, campaigns, cancellationToken: token));
            }
        }

        /// <summary>
        /// Commits a list of ad items to our database that belong to a single campaign.
        /// </summary>
        /// <param name="aditems">The list of ad items</param>
        /// <param name="campaignGuid">The guid of the campaign for these ad items</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="updateStatus">If we want to update the items status and 
        /// approval state</param>
        /// <returns>Task</returns>
        public async Task CommitAdItemBulk(IEnumerable<AdItem> aditems,
            Guid campaignGuid, CancellationToken token, bool updateStatus = false)
        {
            if (aditems == null || aditems.Count() <= 0) { return; }
            if (campaignGuid == null || campaignGuid.Equals(Guid.Empty))
            {
                throw new ArgumentException($"Can't syncback ad items without" +
                    "a valid campaign GUID: {campaignGuid}.");
            }

            var sql = @"
                INSERT INTO 
                    public.ad_item AS INCLUDED (
                        secondary_id, 
                        url, 
                        image_url,
                        title, 
                        approval_state,
                        status, 
                        spent, 
                        clicks, 
                        impressions, 
                        actions, 
                        cpc, 

                        details, 

                        content, 
                        campaign_guid,
                        ad_group_guid,
                        ad_group_image_index,
                        ad_group_title_index,
                        modified_beyond_ad_group)
                VALUES
                    (
                        @SecondaryId,
                        @Url,
                        @ImageUrl,
                        LEFT(@Title, 128),
                        CAST (@ApprovalStateText AS approval_state),
                        CAST (@StatusText AS ad_item_status),
                        @Spent,
                        @Clicks,
                        @Impressions,
                        @Actions,
                        @Cpc,

                        CAST (@Details AS json),

                        @Content,
                        '" + campaignGuid + @"',
                        @AdGroupGuid,
                        @AdGroupImageIndex,
                        @AdGroupTitleIndex,
                        @ModifiedBeyondAdGroup
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
            using (var connection = _dbProvider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    sql, aditems, cancellationToken: token));
            }
        }

        /// <summary>
        /// Commits a list of ad items to our database that belong to a single campaign.
        /// </summary>
        /// <param name="aditems">The list of ad items</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="updateStatus">If we want to update the items status and 
        /// approval state</param>
        /// <returns>Task</returns>
        public async Task CommitAdItemReportsBulk(IEnumerable<AdItem> aditems,
            CancellationToken token, bool updateStatus = false)
        {
            if (aditems == null || aditems.Count() <= 0) { return; }

            var sql = @"
                INSERT INTO 
                    public.ad_item AS INCLUDED (
                        secondary_id, 
                        url, 
                        image_url,
                        title, 

                        spent, 
                        clicks, 
                        impressions, 
                        actions, 
                        cpc)
                VALUES
                    (
                        @SecondaryId,
                        @Url,
                        @ImageUrl,
                        LEFT(@Title, 128),

                        @Spent,
                        @Clicks,
                        @Impressions,
                        @Actions,
                        @Cpc
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

            try
            {
                // Call for execution
                using (var connection = _dbProvider.ConnectionScope())
                {
                    await connection.ExecuteAsync(new CommandDefinition(
                        sql, aditems, cancellationToken: token));
                }
            } catch (Exception e) { throw e; }
        }


        /// <summary>
        /// Commits our accounts to the database.
        /// </summary>
        /// <param name="accounts">The core accounts</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Task</returns>
        public async Task CommitAccountBulk(IEnumerable<Account> accounts,
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

            using (var connection = _dbProvider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    sql, accounts, cancellationToken: token));
            }
        }

    }
}
