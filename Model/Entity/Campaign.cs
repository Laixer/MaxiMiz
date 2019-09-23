using Maximiz.Model.Enums;
using System;

namespace Maximiz.Model.Entity
{
    /// <summary>
    /// Represents a single campaign in some external publisher.
    /// TODO Nullable enums.
    /// </summary>
    [Serializable]
    public class Campaign : EntityAudit<Guid>
    {
        /// <summary>
        /// Network identifier for this object.
        /// </summary>
        public string SecondaryId { get; set; }

        /// <summary>
        /// Group to which this campaign belongs to.
        /// </summary>
        public int CampaignGroup { get; set; }

        /// <summary>
        /// Campaign name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Reference to the publisher.
        /// </summary>
        public Publisher Publisher { get; set; }

        /// <summary>
        /// Indicates the status of any changes made in our own database. These
        /// changes have to be pushed to the corresponding external API.
        /// </summary>
        public ApprovalState ApprovalState { get; set; }

        /// <summary>
        /// Target URL for this campaign. This is where we lead our clicks.
        /// </summary>
        public string TargetUrl { get; set; }

        /// <summary>
        /// Language of the campaign, 2 chars.
        /// TODO Why do we need this?
        /// </summary>
        public string LanguageAsText { get; set; }

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
        public decimal? DailyBudget { get; set; }

        /// <summary>
        /// Indicates our budget model.
        /// </summary>
        public BudgetModel BudgetModel { get; set; }

        /// <summary>
        /// Represents our bid strategy.
        /// </summary>
        public BidStrategy BidStrategy { get; set; }

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
        /// Tracking code.
        /// </summary>
        public string Utm { get; set; }

        /// <summary>
        /// Note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Indicates our status.
        /// TODO This is currently set to ad item status.
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Indicates all devices that this campaign operates on.
        /// </summary>
        public Device[] Devices { get; set; }

        /// <summary>
        /// Indicates all operating systems this campaign operates on.
        /// </summary>
        public OS[] OperatingSystems { get; set; }

        /// <summary>
        /// Indicates all connection types this campaign operates on.
        /// </summary>
        public ConnectionType[] ConnectionTypes { get; set; }

        /// <summary>
        /// JSON string containing unused data which
        /// we do have to store.
        /// </summary>
        public string Details { get; set; }
    }

}
