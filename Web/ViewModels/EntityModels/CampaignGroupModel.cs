using Maximiz.Model.Enums;
using System;

namespace Maximiz.ViewModels.EntityModels
{

    /// <summary>
    /// Represents a campaign group along with some numeric info.
    /// </summary>
    public sealed class CampaignGroupModel : EntityModel
    {

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
        /// Delivery mode of this ad.
        /// </summary>
        public Delivery Delivery { get; set; }

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

    }
}
