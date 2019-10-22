using Maximiz.ViewModels.EntityModels;

namespace Maximiz.ViewModels
{

    /// <summary>
    /// Viewmodel used to display the details for a given <see cref="CampaignModel"/>.
    /// </summary>
    public sealed class CampaignDetailsViewModel
    {

        /// <summary>
        /// The campaign we are displaying.
        /// </summary>
        public CampaignModel Campaign { get; set; }

    }
}
