using Maximiz.Model.Enums;
using System;

namespace Maximiz.Model.Entity
{
    /// <summary>
    /// Represents a campaign group.
    /// TODO Make campaign inherit from this?
    /// </summary>
    [Serializable]
    public class CampaignGroup : EntityAudit<Guid>
    { 
        /// <summary>
        /// Campaign name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Reference to the publisher.
        /// </summary>
        public Publisher Publisher { get; set; }
        public string PublisherText { get => Publisher.GetEnumMemberName(); }

        /// <summary>
        /// Indicates the status of any changes made in our own database. These
        /// changes have to be pushed to the corresponding external API.
        /// </summary>
        public ApprovalState ApprovalState { get; set; }
        public string ApprovalStateText { get => ApprovalState.GetEnumMemberName(); }

        /// <summary>
        /// Delivery mode of this ad.
        /// </summary>
        public Delivery Delivery { get; set; }
        public string DeliveryText { get => Delivery.GetEnumMemberName(); }

        /// <summary>
        /// Campaign branding text.
        /// </summary>
        public string BrandingText { get; set; }

        /// <summary>
        /// Region in which campaign is active.
        /// </summary>
        public int[] LocationInclude { get; set; }

        /// <summary>
        /// Region in which campaign is not active.
        /// </summary>
        public int[] LocationExclude { get; set; }

        /// <summary>
        /// The initial CPC per item.
        /// </summary>
        public decimal InitialCpc { get; set; }

        /// <summary>
        /// Budget per campaign or period.
        /// </summary>
        public decimal Budget { get; set; }

        /// <summary>
        /// Budget per day. Can be null.
        /// </summary>
        public decimal? BudgetDaily { get; set; }

        /// <summary>
        /// Budget spent.
        /// </summary>
        public decimal? Spent { get; set; }

        /// <summary>
        /// Campaign start date.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Campaign end date.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// The <see cref="Account"/> to which this campaign group belongs.
        /// </summary>
        public Guid AccountId { get; set; }

        public Device[] Devices { get; set; }

        public OS[] OperatingSystems { get; set; }

        public BidStrategy BidStrategy { get; set; }
        public string BidStrategyText { get => BidStrategy.GetEnumMemberName(); }

        public string Language { get; set; }

        public string TargetUrl { get; set; }

        public OperationItemStatus OperationItemStatus { get; set; }
        public string OperationItemStatusText { get => OperationItemStatus.GetEnumMemberName(); }

        public Guid OperationId { get; set; }

        public BudgetModel BudgetModel { get; set; }
        public string BudgetModelText { get => BudgetModel.GetEnumMemberName(); }

        public ConnectionType[] ConnectionTypes { get; set; }


        public string Utm { get; set; }

    }

}
