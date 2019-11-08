using Maximiz.ViewModels.EntityModels;
using System;
using System.Collections.Generic;

namespace Maximiz.ViewModels.CampaignDetails
{

    /// <summary>
    /// Viewmodel for displaying all ad groups.
    /// </summary>
    public class AdGroupTableAllViewModel
    {

        /// <summary>
        /// All ad groups.
        /// </summary>
        public IEnumerable<AdGroupModel> AdGroupsAll { get; set; }

        /// <summary>
        /// All ad groups which are linked to the campaign.
        /// </summary>
        public IEnumerable<Guid> AdGroupIdsLinked { get; set; }

        /// <summary>
        /// The total amount of all ad groups based on some query.
        /// Todo separate call maybe?
        /// </summary>
        public int TotalCount { get; set; }

    }
}
