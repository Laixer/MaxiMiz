using Maximiz.Model.Enums;
using System;

namespace Maximiz.Model.Entity
{
    /// <summary>
    /// Represents a <see cref="Campaign"/> along with some calculated statistics.
    /// This entity is obtained from one of our database views.
    /// </summary>
    [Serializable]
    public class CampaignWithStats : Campaign
    {

        // TODO Make sure list of stats is complete.

        /// <summary>
        /// Click through rate, average of all campaign items in this campaign.
        /// </summary>
        public decimal? Ctr { get; set; }

        /// <summary>
        /// Return on investment.
        /// </summary>
        public double? Roi { get; set; }

        /// <summary>
        /// Total revenue of this campaign.
        /// </summary>
        public decimal? Revenue { get; set; }

        /// <summary>
        /// Total revenue from Taboola publishers.
        /// </summary>
        public decimal? RevenueTaboola { get; set; }

        /// <summary>
        /// Total revenue from Adsense publishers.
        /// </summary>
        public decimal? RevenueAdsense { get; set; }

        /// <summary>
        /// Total profit of this campaign.
        /// </summary>
        public decimal? Profit { get; set; }

        /// <summary>
        /// Total actions of all ad items in this campaign.
        /// </summary>
        public int Actions { get; set; }

        /// <summary>
        /// Total amount of clicks as sum of all ad items in this campaign.
        /// </summary>
        public int Clicks { get; set; }

    }

}
