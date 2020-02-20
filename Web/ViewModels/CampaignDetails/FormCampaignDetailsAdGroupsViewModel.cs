using System;
using System.Collections.Generic;

namespace Maximiz.ViewModels.CampaignDetails
{

    /// <summary>
    /// Part of the campaign details viewmodel regarding ad groups.
    /// </summary>
    public sealed partial class FormCampaignDetailsViewModel
    {

        /// <summary>
        /// List of all linked ad groups.
        /// </summary>
        public IEnumerable<Guid> LinkedAdGroupIds { get; set; } = new List<Guid>();

    }
}
