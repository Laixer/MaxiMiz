using Maximiz.ViewModels.Enums;
using System;

namespace Maximiz.ViewModels.CampaignDetails
{

    /// <summary>
    /// Viewmodel to modify our marketing properties for a given campaign.
    /// </summary>
    public sealed class FormCampaignMarketingViewModel
    {

        /// <summary>
        /// The internal id of the campaign we are modifying.
        /// </summary>
        public Guid CampaignId { get; set; }

        /// <summary>
        /// User input cpc.
        /// </summary>
        public decimal Cpc { get; set; }

        /// <summary>
        /// User input bid strategy.
        /// </summary>
        public BidStrategy BidStrategy { get; set; }

        /// <summary>
        /// User input delivery.
        /// </summary>
        public Delivery Delivery { get; set; }

    }
}
