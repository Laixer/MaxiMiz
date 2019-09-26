using Maximiz.Model.Enums;
using Poller.Taboola.Model;

using ApprovalStateInternal = Maximiz.Model.Enums.ApprovalState;
using ApprovalStateExternal = Poller.Taboola.Model.ApprovalState;

using CampaignStatusExternal = Poller.Taboola.Model.CampaignStatus;
using CampaignStatusInternal = Maximiz.Model.Enums.CampaignStatus;
using Poller.Model.Data;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// This partial section contains all explicit conversions for our enums.
    /// Taboola only accepts upper case strings, these functions help us with
    /// converting our enums to this accepted format.
    /// </summary>
    internal partial class MapperCampaign
    {

        /// <summary>
        /// Converts a budgetmodel to a <see cref="SpendingLimitModel"/>
        /// upper case string that the Taboola API understands.
        /// </summary>
        /// <remarks>Default: <see cref="SpendingLimitModel.Entire"/>.</remarks>
        /// <param name="budgetModel">The internal budget model</param>
        /// <returns>The upper case string for external budget model</returns>
        internal string BudgetModelToString(BudgetModel budgetModel)
        {
            var externalBudgetModel = SpendingLimitModel.Entire;

            switch (budgetModel)
            {
                case BudgetModel.Campaign:
                    externalBudgetModel = SpendingLimitModel.Entire;
                    break;
                case BudgetModel.Monthly:
                    externalBudgetModel = SpendingLimitModel.Monthly;
                    break;
            }

            return _utility.ToUpperString(externalBudgetModel);
        }

        /// <summary>
        /// Maps a string representing a <see cref="SpendingLimitModel"/> back
        /// to the enum from upper case, to then map it onto our own model.
        /// </summary>
        /// <remarks>Default: <see cref="BudgetModel.Campaign"/>.</remarks>
        /// <param name="input">Upper case string</param>
        /// <returns>Internal budget model</returns>
        internal BudgetModel BudgetModelToInternal(string input)
        {
            var external = _utility.FromUpperString(input, SpendingLimitModel.Entire);
            switch (external)
            {
                case SpendingLimitModel.Entire:
                    return BudgetModel.Campaign;
                case SpendingLimitModel.Monthly:
                    return BudgetModel.Campaign;
                default:
                    return BudgetModel.Campaign;
            }
        }

        /// <summary>
        /// Converts an internal bid strategy to an exteral upper case string
        /// that represents a <see cref="BidType"/>.
        /// TODO This can not select <see cref="BidType.OptimizedPageviews"/>.
        /// </summary>
        /// <remarks>Default: <see cref="BidType.Fixed"/>.</remarks>
        /// <param name="bidStrategy"></param>
        /// <returns></returns>
        internal string BidStrategyToString(BidStrategy bidStrategy)
        {
            var bidStrategyInternal = BidType.Fixed;

            switch (bidStrategy)
            {
                case BidStrategy.Fixed:
                    bidStrategyInternal = BidType.Fixed;
                    break;
                case BidStrategy.Smart:
                    bidStrategyInternal = BidType.OptimizedConversions;
                    break;
            }

            return _utility.ToUpperString(bidStrategyInternal);
        }

        /// <summary>
        /// Maps a string representing a <see cref="BidType"/> back
        /// to the enum from upper case, to then map it onto our own model.
        /// </summary>
        /// <remarks>Default: <see cref="BidType.Fixed"/>.</remarks>
        /// <param name="upperCaseString">Upper case string</param>
        /// <returns>Bid strategy</returns>
        internal BidStrategy BidStrategyToInternal(string upperCaseString)
        {
            var external = _utility.FromUpperString(upperCaseString, BidType.Fixed);
            switch (external)
            {
                case BidType.Fixed:
                    return BidStrategy.Fixed;
                case BidType.OptimizedConversions:
                    return BidStrategy.Smart;
                case BidType.OptimizedPageviews:
                    return BidStrategy.Smart;
                default:
                    return BidStrategy.Fixed;
            }
        }

        /// <summary>
        /// Converts an internal status to an exteral upper case string
        /// that represents a <see cref="CampaignStatusExternal.PendingApproval"/>.
        /// </summary>
        /// <remarks>Default: <see cref="CampaignStatusExternal.PendingApproval"/>.</remarks>
        /// <param name="internalEnum">The internal status</param>
        /// <returns>Upper case string</returns>
        internal string CampaignStatusToString(CampaignStatusInternal internalEnum)
        {
            var externalEnum = CampaignStatusExternal.PendingApproval;

            switch (internalEnum)
            {
                case CampaignStatusInternal.Unknown:
                    externalEnum = CampaignStatusExternal.PendingApproval;
                    break;
                case CampaignStatusInternal.Running:
                    externalEnum = CampaignStatusExternal.Running;
                    break;
                case CampaignStatusInternal.Paused:
                    externalEnum = CampaignStatusExternal.Paused;
                    break;
                case CampaignStatusInternal.Depleted:
                    externalEnum = CampaignStatusExternal.Depleted;
                    break;
                case CampaignStatusInternal.DepletedMonthly:
                    externalEnum = CampaignStatusExternal.DepletedMonthly;
                    break;
                case CampaignStatusInternal.Expired:
                    externalEnum = CampaignStatusExternal.Expired;
                    break;
                case CampaignStatusInternal.Terminated:
                    externalEnum = CampaignStatusExternal.Terminated;
                    break;
                case CampaignStatusInternal.Frozen:
                    externalEnum = CampaignStatusExternal.Frozen;
                    break;
                case CampaignStatusInternal.PendingApproval:
                    externalEnum = CampaignStatusExternal.PendingApproval;
                    break;
                case CampaignStatusInternal.Rejected:
                    externalEnum = CampaignStatusExternal.Rejected;
                    break;
                case CampaignStatusInternal.PendingStartDate:
                    externalEnum = CampaignStatusExternal.PendingStartDate;
                    break;
            }

            return _utility.ToUpperString(externalEnum);
        }

        /// <summary>
        /// Converts an upper case string that represents an external
        /// <see cref="CampaignStatusExternal"/> to our internal model.
        /// </summary>
        /// <remarks>Default: <see cref="CampaignStatusInternal.Unknown"/>.</remarks>
        /// <param name="upperCaseString">Upper case string</param>
        /// <returns>Internal status</returns>
        internal CampaignStatusInternal CampaignStatusToInternal(string upperCaseString)
        {
            var externalEnum = _utility.FromUpperString(upperCaseString, CampaignStatusExternal.PendingApproval);
            switch (externalEnum)
            {
                case CampaignStatusExternal.Running:
                    return CampaignStatusInternal.Running;
                case CampaignStatusExternal.Paused:
                    return CampaignStatusInternal.Paused;
                case CampaignStatusExternal.PendingStartDate:
                    return CampaignStatusInternal.PendingStartDate;
                case CampaignStatusExternal.DepletedMonthly:
                    return CampaignStatusInternal.DepletedMonthly;
                case CampaignStatusExternal.Depleted:
                    return CampaignStatusInternal.Depleted;
                case CampaignStatusExternal.Expired:
                    return CampaignStatusInternal.Expired;
                case CampaignStatusExternal.Terminated:
                    return CampaignStatusInternal.Terminated;
                case CampaignStatusExternal.Frozen:
                    return CampaignStatusInternal.Frozen;
                case CampaignStatusExternal.PendingApproval:
                    return CampaignStatusInternal.PendingApproval;
                case CampaignStatusExternal.Rejected:
                    return CampaignStatusInternal.Rejected;
                default:
                    return CampaignStatusInternal.Unknown;
            }
        }

        /// <summary>
        /// Converts an internal bid strategy to an exteral upper case string
        /// that represents a <see cref="DailyAdDeliveryModel"/>.
        /// </summary>
        /// <remarks>Default: <see cref="Delivery.Strict"/>.</remarks>
        /// <param name="delivery">The internal delivery</param>
        /// <returns>The external delivery upper case string equivalent</returns>
        internal string DeliveryToString(Delivery delivery)
        {
            var deliveryExternal = Delivery.Strict;

            switch (delivery)
            {
                case Delivery.Balanced:
                    deliveryExternal = Delivery.Balanced;
                    break;
                case Delivery.Accelerated:
                    deliveryExternal = Delivery.Accelerated;
                    break;
                case Delivery.Strict:
                    deliveryExternal = Delivery.Strict;
                    break;
            }

            return _utility.ToUpperString(deliveryExternal);
        }

        /// <summary>
        /// Maps a string representing a <see cref="DailyAdDeliveryModel"/> back
        /// to the enum from upper case, to then map it onto our own model.
        /// </summary>
        /// <remarks>Default: <see cref="Delivery.Strict"/>.</remarks>
        /// <param name="upperCaseString">Upper case string</param>
        /// <returns>Delivery</returns>
        internal Delivery DeliveryToInternal(string upperCaseString)
        {
            var external = _utility.FromUpperString(upperCaseString, DailyAdDeliveryModel.Balanced);
            switch (external)
            {
                case DailyAdDeliveryModel.Balanced:
                    return Delivery.Balanced;
                case DailyAdDeliveryModel.Accelerated:
                    return Delivery.Accelerated;
                case DailyAdDeliveryModel.Strict:
                    return Delivery.Strict;
                default:
                    return Delivery.Strict;
            }
        }

    }
}
