using Dapper;
using Microsoft.Extensions.Logging;
using Poller.Database;
using Poller.Taboola.Traffic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AccountEntity = Maximiz.Model.Entity.Account;
using AdItemEntity = Maximiz.Model.Entity.AdItem;
using CampaignEntity = Maximiz.Model.Entity.Campaign;

namespace Poller.Test.Taboola.Utility
{

    /// <summary>
    /// Used to create, read and delete entities to our own database. This is 
    /// used for testing purposes. This can also clean up externally created
    /// campaigns and ad items.
    /// </summary>
    internal class CrudUtility
    {

        /// <summary>
        /// Communicates with our own database.
        /// </summary>
        private readonly CrudInternal _crudInternal;

        /// <summary>
        /// Communcates with the Taboola API.
        /// </summary>
        private readonly CrudExternal _crudExternal;

        /// <summary>
        /// Logging object.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// DB connection object.
        /// </summary>
        private readonly DbProvider _dbProvider;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger">The logger object</param>
        /// <param name="crudInternal">The internal crud object</param>
        /// <param name="crudExternal">The external crud object</param>
        /// <param name="dbProvider">Database provider object</param>
        public CrudUtility(ILogger logger, CrudInternal crudInternal,
            CrudExternal crudExternal, DbProvider dbProvider)
        {
            _logger = logger;
            _crudInternal = crudInternal;
            _crudExternal = crudExternal;
            _dbProvider = dbProvider;
        }

        /// <summary>
        /// Remove all created dummy campaigns. Use this after a debug session
        /// or test execution in which a lot of dummy campaigns were created.
        /// </summary>
        /// <param name="usedName">The campaign debug name that was used</param>
        public async Task RemoveExternalAllDummyCampaignsAsync(string usedName)
        {
            _logger.LogInformation("Starting dummy removal");
            var source = new CancellationTokenSource();
            var token = source.Token;

            var accounts = await _crudInternal.GetAdvertiserAccountsCachedAsync(token);
            foreach (var account in accounts)
            {
                // Use discard to optimize memory allocation management
                _ = RemoveExternalAccountDummyCampaignsAsync(account, usedName, token);

                // Prevent spamming
                await Task.Delay(250);
            }

            source.Dispose();
            _logger.LogInformation("Completed dummy removal");
        }

        /// <summary>
        /// Remove dummy campaigns for a specific account.
        /// </summary>
        /// <param name="account">The account as internal entity</param>
        /// <param name="usedName">The dummy campaign name used</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        public async Task RemoveExternalAccountDummyCampaignsAsync(AccountEntity account,
            string usedName, CancellationToken token)
        {
            var campaigns = await _crudExternal.GetAllCampaignsFromAccountAsync(account, token);
            var dummies = new List<CampaignEntity>();
            dummies.AddRange(campaigns.Where(x => x.Name.Equals(usedName)).ToList());
            foreach (var dummy in dummies.AsParallel())
            {
                await _crudExternal.DeleteCampaignAsync(account, dummy, token);
            }
        }

        /// <summary>
        /// Removes all dummy campaigns from our internal database.
        /// </summary>
        /// <param name="usedName">The used debug campaign name</param>
        /// <returns>Task</returns>
        public async Task RemoveInternalAllDummyCampaignsAsync(string usedName)
        {
            var sql = $"DELETE FROM public.campaign WHERE name = '{usedName}'";
            using (var connection = _dbProvider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(sql));
            }
        }

        /// <summary>
        /// Removes all dummy ad items from our internal database.
        /// </summary>
        /// <param name="usedTitle">The used ad item title</param>
        /// <returns>Task</returns>
        public async Task RemoveInternalAllDummyAdItemsAsync(string usedTitle)
        {
            var sql = $"DELETE FROM public.ad_item WHERE title = '{usedTitle}'";
            using (var connection = _dbProvider.ConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(sql));
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
        public async Task<CampaignEntity> CommitCampaignWriteGuid(
            CampaignEntity campaign, CancellationToken token)
        {

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
                        CAST (@PublisherText AS publisher),
                        @Language
                    )
                ON CONFLICT (secondary_id) DO NOTHING
                RETURNING Id;";

            using (var connection = _dbProvider.ConnectionScope())
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
        /// Commits a single ad item.
        /// TODO This must return GUID. Same structure.
        /// </summary>
        /// <param name="adItem">The ad item to commit</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        public async Task<AdItemEntity> CommitAdItemWriteGuid(AdItemEntity adItem,
            CancellationToken token)
        {

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
                        ad_group_title_index,
                        ad_group_image_index,
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
                        @CampaignGuid,
                        @AdGroupGuid,
                        @AdGroupTitleIndex,
                        @AdGroupImageIndex,
                        @ModifiedBeyondAdGroup
                    )
                ON CONFLICT (secondary_id) DO NOTHING
                RETURNING Id;";

            using (var connection = _dbProvider.ConnectionScope())
            {
                var guid = await connection.ExecuteScalarAsync<Guid>(
                    new CommandDefinition(sql, adItem, cancellationToken: token));

                // TODO This should be done differently
                if (adItem.Id == Guid.Empty || adItem.Id == null)
                {
                    adItem.Id = guid;
                }
                return adItem;
            }
        }

    }
}
