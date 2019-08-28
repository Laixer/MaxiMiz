using Maximiz.Model.Enums;
using System;

namespace Maximiz.Model.Entity
{
    /// <summary>
    /// Campaign.
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
        /// TODO Why do we need this? Should just be a string.
        /// </summary>
        public string[] Language { get; set; }

        /// <summary>
        /// Targeted Device.
        /// </summary>
        public Device[] Device { get; set; }

        /// <summary>
        /// Targeted OS.
        /// </summary>
        public OS[] Os { get; set; }

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
        /// Budget model for the campaign.
        /// </summary>
        public BudgetModel BudgetModel { get; set; }

        /// <summary>
        /// Delivery mode of this ad.
        /// </summary>
        public Delivery Delivery { get; set; }
        public string DeliveryText { get => Delivery.GetEnumMemberName(); }

        /// <summary>
        /// Bid Strategy.
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
        /// Budget spent.
        /// </summary>
        public decimal Spent { get; set; }

        /// <summary>
        /// Note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// JSON string containing unused data which
        /// we do have to store.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Connections.
        /// </summary>
        public Connection[] Connection { get; set; }

        /// <summary>
        /// Returns a new Campaign entity created from Campaign Group Input.
        /// </summary>
        /// <param name="group">The campaign group this campaign should belong to.</param>
        public static Campaign FromGroup(CampaignGroup group)
        {
            return new Campaign
            {
                Name = group.Name,
                BrandingText = group.BrandingText,
                LocationInclude = group.LocationInclude,
                LocationExclude = group.LocationExclude,
                Language = new string[] { group.Language },
                InitialCpc = group.InitialCpc,
                Budget = group.Budget,
                DailyBudget = group.DailyBudget,
                BudgetModel = group.BudgetModel,
                Delivery = group.Delivery,
                BidStrategy = group.BidStrategy,
                StartDate = group.StartDate,
                EndDate = group.EndDate,
                Status = group.Status,
                Utm = group.Utm,
                Note = group.Note,
                Connection = group.Connection
            };
        }
    }

}
