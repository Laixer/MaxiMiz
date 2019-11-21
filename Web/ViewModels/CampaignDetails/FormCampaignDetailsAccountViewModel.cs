using System.ComponentModel.DataAnnotations;

namespace Maximiz.ViewModels.CampaignDetails
{

    /// <summary>
    /// Partial viewmodel that contains the account modifications for a campaign.
    /// </summary>
    public sealed partial class FormCampaignDetailsViewModel
    {

        /// <summary>
        /// Name of this campaign group, human readable. This will be added onto
        /// all generated names for each campaign.
        /// </summary>
        [Required]
        public string CampaignNameSuffix { get; set; }

        /// <summary>
        /// User input branding text.
        /// </summary>
        [Required]
        public string BrandingText { get; set; }

        /// <summary>
        /// User input campaign target url.
        /// </summary>
        [Required]
        public string Url { get; set; }

        /// <summary>
        /// User input campaign utm custom parameters.
        /// </summary>
        public string UtmCustom { get; set; }

    }
}
