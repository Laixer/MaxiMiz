using System;

namespace Maximiz.ViewModels.CampaignDetails
{

    /// <summary>
    /// Viewmodel to represent a linking operation between a campaign and an ad group.
    /// </summary>
    public sealed class LinkingOperationViewModel
    {

        /// <summary>
        /// Campaign internal id.
        /// </summary>
        public Guid CampaignId { get; set; }

        /// <summary>
        /// Ad group internal id.
        /// </summary>
        public Guid AdGroupId { get; set; }

    }
}
