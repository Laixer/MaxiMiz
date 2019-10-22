using Dapper;
using Laixer.Library.Injection.Database;
using Maximiz.Model.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Transactions.CreateUpdateDelete
{

    /// <summary>
    /// Performs CUD operations for <see cref="CampaignGroup"/>s in our database.
    /// </summary>
    internal class CudCampaignGroup : ICud<CampaignGroup>
    {

        /// <summary>
        /// Provides database connections for us.
        /// </summary>
        private IDatabaseProvider databaseProvider;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="provider"><see cref="IDatabaseProvider"/></param>
        public CudCampaignGroup(IDatabaseProvider provider)
        {
            databaseProvider = provider;
        }

        /// <summary>
        /// Creates a campaign group in our internal database.
        /// </summary>
        /// <param name="entity">The campaign group to create</param>
        /// <returns>The created campaign group</returns>
        public async Task<CampaignGroup> Create(CampaignGroup entity)
        {
            var sql = @"
               INSERT INTO
	                public.campaign_group(
                        name, 
                        branding_text, 
                        location_include, 
                        location_exclude, 
                        language,
                        devices,
                        operating_systems,
                        initial_cpc, 
                        budget, 
                        budget_daily,
                        budget_model,
                        delivery,
                        bid_strategy,
                        start_date, 
                        end_date,
                        note, 
                        connection_types,
                        approval_state,
                        publisher,
                        spent, 
                        target_url
                    )
                VALUES
                    (
                        @Name,
                        @BrandingText,
                        @LocationInclude,
                        @LocationExclude,
                        @Language,
                        @Devices,
                        @OperatingSystems,
                        @InitialCpc,
                        @Budget,
                        @BudgetDaily,
                        CAST (@BudgetModelText AS budget_model),
                        CAST (@DeliveryText AS delivery),
                        CAST (@BidStrategyText AS bid_strategy),
                        @StartDate,
                        VALIDATE_TIMESTAMP(@EndDate),
                        @Note,
                        @ConnectionTypes,
                        CAST (@ApprovalStateText AS approval_state),
                        CAST (@PublisherText AS publisher),
                        @TargetUrl,
                        @Spent
                    )
                RETURNING Id;";

            using (var connection = databaseProvider.GetConnectionScope())
            {
                var guid = await connection.ExecuteScalarAsync<Guid>(new CommandDefinition(sql, entity));
                var retrieve = $"SELECT * FROM public.campaign WHERE id::text='{guid.ToString()}';";
                var result = (await connection.QueryAsync<CampaignGroup>(new CommandDefinition(sql))).FirstOrDefault();

                // TODO Define exception, maybe do differently
                return result ?? throw new Exception("Something went wrong while creating campaign group in local database");
            }
        }

        public Task<CampaignGroup> Update(CampaignGroup entity)
        {
            throw new NotImplementedException();
        }

        public Task<CampaignGroup> Delete(CampaignGroup entity)
        {
            throw new NotImplementedException();
        }

    }
}
