using Maximiz.Model.Enums;
using System;

namespace Maximiz.Model.Entity
{
    /// <summary>
    /// Campaign Group.
    /// </summary>
    [Serializable]
    public class CampaignGroup : EntityAudit<int>
    {
        /// <summary>
        /// Campaign name.
        /// </summary>
        public string Name { get; set; }

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
        /// Language of the campaign, 2 chars.
        /// TODO Why do we need this?
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Targeted devices.
        /// </summary>
        public Device[] Device { get; set; }

        /// <summary>
        /// Targeted operating systems.
        /// </summary>
        public OS[] OS { get; set; }

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
        /// Budget Model.
        /// </summary>
        public BudgetModel BudgetModel { get; set; }

        /// <summary>
        /// Delivery mode of this ad.
        /// </summary>
        public Delivery Delivery { get; set; }
        public string DeliveryText { get => Delivery.GetEnumMemberName(); }

        /// <summary>
        /// Bid strategy.
        /// </summary>
        public BidStrategy BidStrategy { get; set; }

        /// <summary>
        /// Campaign start date.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Campaign end date.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Status.
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Tracking code.
        /// </summary>
        public string Utm { get; set; }

        /// <summary>
        /// Note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Connections.
        /// </summary>
        public Connection[] Connection { get; set; }
    }
}
