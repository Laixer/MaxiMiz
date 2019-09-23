using Dapper;
using Maximiz.Model.Entity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Poller.Database;
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

    }
}
