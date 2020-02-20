using static Maximiz.Infrastructure.Querying.QueryExtractor;
using static Maximiz.Infrastructure.Querying.DatabaseColumnMap;
using static Maximiz.Infrastructure.Querying.PropertyUtility;
using Maximiz.Model.Entity;

namespace Maximiz.Infrastructure.Committing.Sql
{

    /// <summary>
    /// Contains SQL statements for <see cref="CampaignGroup"/>s.
    /// </summary>
    internal static partial class Sql
    {

        /// <summary>
        /// Sql statement to create a campaign group and return its internal id.
        /// </summary>
        /// <returns>SQL statement</returns>
        internal static string SqlCreateCampaignGroup()
            => $"INSERT INTO {GetTableName<CampaignGroup>()} (" +
            $" {CampaignGroup(x => x.AccountId)}," +
            $" {CampaignGroup(x => x.ApprovalState)}," +
            $" {CampaignGroup(x => x.BidStrategy)}," +
            $" {CampaignGroup(x => x.BrandingText)}," +
            $" {CampaignGroup(x => x.Budget)}," +
            $" {CampaignGroup(x => x.BudgetDaily)}," +
            $" {CampaignGroup(x => x.BudgetModel)}," +
            $" {CampaignGroup(x => x.ConnectionTypes)}," +
            $" {CampaignGroup(x => x.Delivery)}," +
            $" {CampaignGroup(x => x.Devices)}," +
            $" {CampaignGroup(x => x.EndDate)}," +
            $" {CampaignGroup(x => x.InitialCpc)}," +
            $" {CampaignGroup(x => x.Language)}," +
            $" {CampaignGroup(x => x.LocationExclude)}," +
            $" {CampaignGroup(x => x.LocationInclude)}," +
            $" {CampaignGroup(x => x.Name)}," +
            $" {CampaignGroup(x => x.OperatingSystems)}," +
            $" {CampaignGroup(x => x.OperationId)}," +
            $" {CampaignGroup(x => x.OperationItemStatus)}," +
            $" {CampaignGroup(x => x.Publisher)}," +
            $" {CampaignGroup(x => x.Spent)}," +
            $" {CampaignGroup(x => x.StartDate)}," +
            $" {CampaignGroup(x => x.TargetUrl)}," +
            $" {CampaignGroup(x => x.Utm)}" +
            $") VALUES (" +
            $" @{GetName<CampaignGroup>(x => x.AccountId)}," +
            $" CAST (@{GetName<CampaignGroup>(x => x.ApprovalStateText)} AS approval_state)," +
            $" CAST (@{GetName<CampaignGroup>(x => x.BidStrategyText)} AS bid_strategy)," +
            $" @{GetName<CampaignGroup>(x => x.BrandingText)}," +
            $" @{GetName<CampaignGroup>(x => x.Budget)}," +
            $" @{GetName<CampaignGroup>(x => x.BudgetDaily)}," +
            $" CAST (@{GetName<CampaignGroup>(x => x.BudgetModelText)} AS budget_model)," +
            $" @{GetName<CampaignGroup>(x => x.ConnectionTypes)}," +
            $" CAST (@{GetName<CampaignGroup>(x => x.DeliveryText)} AS delivery)," +
            $" @{GetName<CampaignGroup>(x => x.Devices)}," +
            $" @{GetName<CampaignGroup>(x => x.EndDate)}," +
            $" @{GetName<CampaignGroup>(x => x.InitialCpc)}," +
            $" @{GetName<CampaignGroup>(x => x.Language)}," +
            $" @{GetName<CampaignGroup>(x => x.LocationExclude)}," +
            $" @{GetName<CampaignGroup>(x => x.LocationInclude)}," +
            $" @{GetName<CampaignGroup>(x => x.Name)}," +
            $" @{GetName<CampaignGroup>(x => x.OperatingSystems)}," +
            $" @{GetName<CampaignGroup>(x => x.OperationId)}," +
            $" CAST (@{GetName<CampaignGroup>(x => x.OperationItemStatusText)} AS operation_item_status)," +
            $" CAST (@{GetName<CampaignGroup>(x => x.PublisherText)} AS publisher)," +
            $" @{GetName<CampaignGroup>(x => x.Spent)}," +
            $" @{GetName<CampaignGroup>(x => x.StartDate)}," +
            $" @{GetName<CampaignGroup>(x => x.TargetUrl)}," +
            $" @{GetName<CampaignGroup>(x => x.Utm)}" +
            $") RETURNING {CampaignGroup(x => x.Id)};";

        /// <summary>
        /// Sql statement to update a campaign group.
        /// </summary>
        /// <returns>SQL statement</returns>
        internal static string SqlUpdateCampaignGroup()
            => $"UPDATE {GetTableName<CampaignGroup>()} SET " +
            $" {UpdateEquality<CampaignGroup>(x => x.AccountId)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.ApprovalState)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.BidStrategy)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.BrandingText)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.Budget)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.BudgetDaily)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.BudgetModel)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.ConnectionTypes)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.Delivery)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.Devices)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.EndDate)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.InitialCpc)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.Language)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.LocationExclude)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.LocationInclude)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.Name)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.OperatingSystems)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.OperationItemStatus)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.OperationId)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.Publisher)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.Spent)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.StartDate)}," +
            $" {UpdateEquality<CampaignGroup>(x => x.TargetUrl)}" +
            $" WHERE {CampaignGroup(x => x.Id)} = @{GetName<CampaignGroup>(x => x.Id)};";
    }

}
