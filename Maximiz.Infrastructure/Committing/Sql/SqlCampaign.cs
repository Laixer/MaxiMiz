using static Maximiz.Infrastructure.Querying.QueryExtractor;
using static Maximiz.Infrastructure.Querying.DatabaseColumnMap;
using static Maximiz.Infrastructure.Querying.PropertyUtility;
using Maximiz.Model.Entity;

namespace Maximiz.Infrastructure.Committing.Sql
{

    /// <summary>
    /// Contains sql statements for our <see cref="Core.Infrastructure.Commiting.ICommitter{TEntity}"/>s.
    /// </summary>
    internal static partial class Sql
    {

        /// <summary>
        /// Sql statement to create a campaign and return its internal id.
        /// </summary>
        /// <returns>SQL statement</returns>
        internal static string SqlCreateCampaign()
            => $"INSERT INTO {GetTableName<Campaign>()} (" +
            $" {Campaign(x => x.AccountGuid)}," +
            $" {Campaign(x => x.ApprovalState)}," +
            $" {Campaign(x => x.BidStrategy)}," +
            $" {Campaign(x => x.BrandingText)}," +
            $" {Campaign(x => x.Budget)}," +
            $" {Campaign(x => x.BudgetDaily)}," +
            $" {Campaign(x => x.BudgetModel)}," +
            $" {Campaign(x => x.CampaignGroupGuid)}," +
            $" {Campaign(x => x.ConnectionTypes)}," +
            $" {Campaign(x => x.Delivery)}," +
            $" {Campaign(x => x.Devices)}," +
            $" {Campaign(x => x.Details)}," +
            $" {Campaign(x => x.EndDate)}," +
            $" {Campaign(x => x.InitialCpc)}," +
            $" {Campaign(x => x.Language)}," +
            $" {Campaign(x => x.LocationExclude)}," +
            $" {Campaign(x => x.LocationInclude)}," +
            $" {Campaign(x => x.Name)}," +
            $" {Campaign(x => x.Note)}," +
            $" {Campaign(x => x.OperatingSystems)}," +
            $" {Campaign(x => x.OperationId)}," +
            $" {Campaign(x => x.OperationItemStatus)}," +
            $" {Campaign(x => x.Publisher)}," +
            $" {Campaign(x => x.SecondaryId)}," +
            $" {Campaign(x => x.Spent)}," +
            $" {Campaign(x => x.StartDate)}," +
            $" {Campaign(x => x.Status)}," +
            $" {Campaign(x => x.TargetUrl)}," +
            $" {Campaign(x => x.Utm)}" +
            $" )" +
            $" VALUES" +
            $" (" +
            $" @{GetName<Campaign>(x => x.AccountGuid)}," +
            $" CAST (@{GetName<Campaign>(x => x.ApprovalStateText)} AS approval_state)," +
            $" CAST (@{GetName<Campaign>(x => x.BidStrategyText)} AS bid_strategy)," +
            $" @{GetName<Campaign>(x => x.BrandingText)}," +
            $" @{GetName<Campaign>(x => x.Budget)}," +
            $" @{GetName<Campaign>(x => x.BudgetDaily)}," +
            $" CAST (@{GetName<Campaign>(x => x.BudgetModelText)} AS budget_model)," +
            $" @{GetName<Campaign>(x => x.CampaignGroupGuid)}," +
            $" @{GetName<Campaign>(x => x.ConnectionTypes)}," +
            $" CAST (@{GetName<Campaign>(x => x.DeliveryText)} AS delivery)," +
            $" @{GetName<Campaign>(x => x.Devices)}," +
            $" CAST (@{GetName<Campaign>(x => x.Details)} AS json)," +
            $" VALIDATE_TIMESTAMP(@{GetName<Campaign>(x => x.EndDate)})," +
            $" @{GetName<Campaign>(x => x.InitialCpc)}," +
            $" @{GetName<Campaign>(x => x.Language)}," +
            $" @{GetName<Campaign>(x => x.LocationExclude)}," +
            $" @{GetName<Campaign>(x => x.LocationInclude)}," +
            $" @{GetName<Campaign>(x => x.Name)}," +
            $" @{GetName<Campaign>(x => x.Note)}," +
            $" @{GetName<Campaign>(x => x.OperatingSystems)}," +
            $" @{GetName<Campaign>(x => x.OperationId)}," +
            $" CAST (@{GetName<Campaign>(x => x.OperationItemStatusText)} AS operation_item_status)," +
            $" CAST (@{GetName<Campaign>(x => x.PublisherText)} AS publisher)," +
            $" @{GetName<Campaign>(x => x.SecondaryId)}," +
            $" @{GetName<Campaign>(x => x.Spent)}," +
            $" @{GetName<Campaign>(x => x.StartDate)}," +
            $" CAST (@{GetName<Campaign>(x => x.StatusText)} AS campaign_status)," +
            $" @{GetName<Campaign>(x => x.TargetUrl)}," +
            $" @{GetName<Campaign>(x => x.Utm)}" +
            $" )" +

            //$" ;";            
            $" RETURNING {Campaign(x => x.Id)};";

        /// <summary>
        /// Sql statement to update a campaign.
        /// </summary>
        /// <returns>SQL statement</returns>
        internal static string SqlUpdateCampaign()
            => $"UPDATE {GetTableName<Campaign>()} SET" +
            $" {Campaign(x => x.AccountGuid)} = @{GetName<Campaign>(x => x.AccountGuid)}," +
            $" {Campaign(x => x.ApprovalState)} = @{GetName<Campaign>(x => x.ApprovalState)}," +
            $" {Campaign(x => x.BidStrategy)} = @{GetName<Campaign>(x => x.BidStrategy)}," +
            $" {Campaign(x => x.BrandingText)} = @{GetName<Campaign>(x => x.BrandingText)}," +
            $" {Campaign(x => x.Budget)} = @{GetName<Campaign>(x => x.Budget)}," +
            $" {Campaign(x => x.BudgetDaily)} = @{GetName<Campaign>(x => x.BudgetDaily)}," +
            $" {Campaign(x => x.BudgetModel)} = @{GetName<Campaign>(x => x.BudgetModel)}," +
            $" {Campaign(x => x.CampaignGroupGuid)} = @{GetName<Campaign>(x => x.CampaignGroupGuid)}," +
            $" {Campaign(x => x.ConnectionTypes)} = @{GetName<Campaign>(x => x.ConnectionTypes)}," +
            $" {Campaign(x => x.Delivery)} = @{GetName<Campaign>(x => x.Delivery)}," +
            $" {Campaign(x => x.Details)} = @{GetName<Campaign>(x => x.Details)}," + // TODO This just overwrites now
            $" {Campaign(x => x.Devices)} = @{GetName<Campaign>(x => x.Devices)}," +
            $" {Campaign(x => x.EndDate)} = @{GetName<Campaign>(x => x.EndDate)}," +
            // Skipping id
            $" {Campaign(x => x.InitialCpc)} = @{GetName<Campaign>(x => x.InitialCpc)}," +
            $" {Campaign(x => x.Language)} = @{GetName<Campaign>(x => x.Language)}," +
            $" {Campaign(x => x.LocationExclude)} = @{GetName<Campaign>(x => x.LocationExclude)}," +
            $" {Campaign(x => x.LocationInclude)} = @{GetName<Campaign>(x => x.LocationInclude)}," +
            $" {Campaign(x => x.Name)} = @{GetName<Campaign>(x => x.Name)}," +
            $" {Campaign(x => x.Note)} = @{GetName<Campaign>(x => x.Note)}," +
            $" {Campaign(x => x.OperatingSystems)} = @{GetName<Campaign>(x => x.OperatingSystems)}," +
            $" {Campaign(x => x.OperationItemStatus)} = @{GetName<Campaign>(x => x.OperationItemStatus)}," +
            $" {Campaign(x => x.Publisher)} = @{GetName<Campaign>(x => x.Publisher)}," +
            $" {Campaign(x => x.SecondaryId)} = @{GetName<Campaign>(x => x.SecondaryId)}," +
            $" {Campaign(x => x.Spent)} = @{GetName<Campaign>(x => x.Spent)}," +
            $" {Campaign(x => x.StartDate)} = @{GetName<Campaign>(x => x.StartDate)}," +
            $" {Campaign(x => x.Status)} = @{GetName<Campaign>(x => x.Status)}," +
            $" {Campaign(x => x.TargetUrl)} = @{GetName<Campaign>(x => x.TargetUrl)}," +
            $" {Campaign(x => x.Utm)} = @{GetName<Campaign>(x => x.Utm)}" +
            $" WHERE {Campaign(x => x.Id)} = @{GetName<Campaign>(x => x.Id)};";

    }
}
