using Maximiz.ViewModels.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maximiz.ViewModels.CampaignDetails
{

    /// <summary>
    /// Viewmodel to modify our targeting properties for a given campaign.
    /// </summary>
    public sealed partial class FormCampaignDetailsViewModel
    {

        /// <summary>
        /// All countries we want to target.
        /// </summary>
        [Required]
        public List<Location> Locations { get; set; }

        /// <summary>
        /// All devices we want to target.
        /// </summary>
        [Required]
        public IEnumerable<Device> Devices { get; set; }

        /// <summary>
        /// All operating systems we want to target.
        /// </summary>
        [Required]
        public IEnumerable<OS> OperatingSystems { get; set; }

    }
}
