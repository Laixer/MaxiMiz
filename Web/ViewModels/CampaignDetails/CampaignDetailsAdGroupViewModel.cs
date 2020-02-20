using Maximiz.ViewModels.EntityModels;
using System.Collections.Generic;

namespace Maximiz.ViewModels.CampaignDetails
{

    /// <summary>
    /// Viewmodel used for the ad group tab of our campaign details view.
    /// </summary>
    public sealed class CampaignDetailsAdGroupViewModel
    {

        /// <summary>
        /// The campaign we are looking at.
        /// </summary>
        public CampaignModel Campaign { get; set; }

        /// <summary>
        /// Total amount of ad groups that exist in the database.
        /// </summary>
        public int AdGroupsTotal { get; set; }

    }
}
