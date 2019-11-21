using Maximiz.ViewModels.EntityModels;
using System.Collections.Generic;

namespace Maximiz.ViewModels.CampaignGroupWizard
{

    /// <summary>
    /// Viewmodel for displaying a list of ad groups in the cmapaign group wizard.
    /// </summary>
    public sealed class CampaignGroupWizardAdGroupViewModel
    {

        /// <summary>
        /// All retrieved ad groups.
        /// </summary>
        public IEnumerable<AdGroupModel> AdGroupList { get; set; }

    }
}
