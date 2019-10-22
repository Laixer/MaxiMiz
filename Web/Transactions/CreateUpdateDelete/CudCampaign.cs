using Dapper;
using Laixer.Library.Injection.Database;
using Maximiz.Model.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Transactions.CreateUpdateDelete
{

    /// <summary>
    /// Performs CUD operations for <see cref="Campaign"/>s in our database.
    /// </summary>
    internal class CudCampaign : ICud<Campaign>
    {

        /// <summary>
        /// Provides database connections for us.
        /// </summary>
        private IDatabaseProvider databaseProvider;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="provider"><see cref="IDatabaseProvider"/>.</param>
        public CudCampaign(IDatabaseProvider provider)
        {
            databaseProvider = provider;
        }

        /// <summary>
        /// Creates a campaign in our internal database.
        /// </summary>
        /// <param name="entity">The campaign to create</param>
        /// <returns>The created campaign</returns>
        public async Task<Campaign> Create(Campaign entity)
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
                        language
                    )
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
                RETURNING Id;";

            using (var connection = databaseProvider.GetConnectionScope())
            {
                var guid = await connection.ExecuteScalarAsync<Guid>(new CommandDefinition(sql, entity));
                var retrieve = $"SELECT * FROM public.campaign WHERE id::text='{guid.ToString()}';";
                var result = (await connection.QueryAsync<Campaign>(new CommandDefinition(sql))).FirstOrDefault();

                // TODO Define exception, maybe do differently
                return result ?? throw new Exception("Something went wrong while creating campaign in local database");
            }
        }

        public Task<Campaign> Update(Campaign entity)
        {
            throw new NotImplementedException();
        }
        public Task<Campaign> Delete(Campaign entity)
        {
            throw new NotImplementedException();
        }

    }
}
