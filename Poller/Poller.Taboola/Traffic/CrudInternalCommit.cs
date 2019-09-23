using Dapper;
using Maximiz.Model.Entity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Poller.Database;
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
                    location_include = @LocationInclude, 
                    location_exclude = @LocationExclude,
                    language_as_text = @LanguageAsText, 
                    device = CAST (@Device as device[]),
                    operating_systems = CAST (@OperatingSystems as operating_system[]), 
                    initial_cpc = @InitialCpc, 
                    budget = @Budget, 
                    budget_daily = @DailyBudget, 
                    budget_model = CAST (@BudgetModel as budget_model),
                    delivery = CAST (@DeliveryText AS delivery), 
                    bid_strategy = CASE (@BidStrategy AS bid_strategy),
                    start_date = COALESCE(@StartDate, CURRENT_TIMESTAMP),     
                    end_date = VALIDATE_TIMESTAMP(@EndDate), 
                    utm = @Utm, 
                    status = CAST (@Status as ad_item_status),
                    note = @Note, 
                    campaign_group = @CampaignGroup, 
                    connection_types = CAST (@ConnectionTypes as connection[]),
                    spent = @Spent, 
                    details = CAST (@Details AS json),
                    approval_state = CAST (@ApprovalState as approval_state)
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
                UPDATE public.campaign
	            SET
                    secondary_id = @SecondaryId,
                    ad_group_id = @AdGroupId,
                    title = @Title,
                    url = @Url,
                    content = @Content,
                    cpc = @Cpc,
                    spent = @Spent,
                    clicks = @Clicks,
                    impressions = @Impressions,
                    actions = @Actions,
                    details = CAST (@Details AS json),
                    status = CAST (@StatusText AS ad_item_status),
                    approval_state = CAST (@ApprovalStateText as approval_state),
                    campaign_guid = @CampaignGuid,
                    ad_group_image_index = @AdGroupImageIndex,
                    ad_group_title_index = @AdGroupTitleIndex
                WHERE Id = @Id;";

            using (var connection = _dbProvider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    sql, adItem, cancellationToken: token));
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
        /// TODO Revise
        /// This commits our fetched campaigns to our local database.
        /// </summary>
        /// <param name="campaigns">Core campaign list</param>
        /// <param name="token"></param>
        /// <returns>Task</returns>
        private async Task CommitCampaignBulk(IEnumerable<Campaign> campaigns,
            CancellationToken token)
        {
            if (campaigns == null || campaigns.ToList().Count() <= 0) { return; }

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

            using (var connection = _dbProvider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    sql, campaigns, cancellationToken: token));
            }
        }

        /// <summary>
        /// TODO Revise.
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
        private async Task CommitAdItemBulk(IEnumerable<AdItem> aditems,
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
            using (var connection = _dbProvider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    sql, aditems, cancellationToken: token));
            }
        }

        /// <summary>
        /// Commits our accounts to the database.
        /// </summary>
        /// <param name="accounts">The core accounts</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Nothing (task)</returns>
        private async Task CommitAccountBulk(IEnumerable<Account> accounts,
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
