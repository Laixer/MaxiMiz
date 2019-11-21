using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maximiz.ViewModels.CampaignGroupWizard
{

    /// <summary>
    /// Viewmodel for the ad group tab of creating a new campaign group.
    /// </summary>
    public sealed partial class CampaignGroupFormAllViewModel
    {

        /// <summary>
        /// List of selected ad group ids to link our campaign group with.
        /// </summary>
        [Required]
        public IEnumerable<Guid> SelectedAdGroupIds { get; set; }

    }

}
