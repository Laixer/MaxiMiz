
namespace Maximiz.Infrastructure.Committing.Sql
{

    /// <summary>
    /// Contains sql statements for our <see cref="CampaignCommitter"/>.
    /// </summary>
    internal static class SqlCampaignCommitter
    {

        /// <summary>
        /// Sql statement to create a campaign and return its internal id.
        /// </summary>
        public static string CreateCampaign { get; } = @"
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

        /// <summary>
        /// Sql statement to get a campaign based on its id.
        /// </summary>
        public static string GetCampaign { get; } = @"GET 1 FROM public.campaign WHERE id = @Id";

        /// <summary>
        /// Sql statement to update a campaign.
        /// </summary>
        public static string UpdateCampaign { get; } = @"
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

        /// <summary>
        /// Sql statement to delete a campaign.
        /// </summary>
        public static string DeleteCampaign { get; } = @"DELETE FROM public.campaign WHERE id = @Id";

    }
}
