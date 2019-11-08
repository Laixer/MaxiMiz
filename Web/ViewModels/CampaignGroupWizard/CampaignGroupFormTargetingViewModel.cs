using Maximiz.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maximiz.ViewModels.NewCampaignGroup
{

    /// <summary>
    /// View model for the targeting form for our campaign group creation.
    /// </summary>
    public sealed partial class CampaignGroupFormAllViewModel
    {

        /// <summary>
        /// All countries we want to target.
        /// </summary>
        [Required]
        public IEnumerable<Location> Locations { get; set; }

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
