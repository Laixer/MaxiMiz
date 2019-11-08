using Maximiz.ViewModels.EntityModels;
using System.Collections.Generic;

namespace Maximiz.ViewModels.CampaignDetails
{

    /// <summary>
    /// Viewmodel for the linked ad group table.
    /// </summary>
    public class AdGroupTableLinkedViewModel
    {

        /// <summary>
        /// All linked ad groups
        /// </summary>
        public IEnumerable<AdGroupModel> AdGroups { get; set; }

        /// <summary>
        /// The total amount of linked ad groups based on some query.
        /// TODO Not used right now.
        /// </summary>
        public int TotalCount { get; set; }

    }
}
