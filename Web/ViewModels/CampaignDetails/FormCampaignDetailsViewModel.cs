using System;

namespace Maximiz.ViewModels.CampaignDetails
{

    /// <summary>
    /// Viewmodel for our modification form of a campaign details page.
    /// TODO Do we want this partial?
    /// </summary>
    public sealed partial class FormCampaignDetailsViewModel
    {

        /// <summary>
        /// The internal id of the campaign we are modifying.
        /// </summary>
        public Guid CampaignId { get; set; }

    }
}
