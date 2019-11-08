using Maximiz.ViewModels.Enums;
using System;

namespace Maximiz.ViewModels.CampaignDetails
{

    /// <summary>
    /// Viewmodel to contain our modification properties for a given campaign.
    /// </summary>
    public sealed class FormCampaignAccountViewModel
    {

        /// <summary>
        /// The internal id of the campaign we are modifying.
        /// </summary>
        public Guid CampaignId { get; set; }

        /// <summary>
        /// User input campaign name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User input branding text.
        /// </summary>
        public string BrandingText { get; set; }

        /// <summary>
        /// User input campaign target url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// User input campaign utm custom parameters.
        /// </summary>
        public string UtmCustom { get; set; }

    }
}
