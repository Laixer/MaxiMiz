﻿using Maximiz.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maximiz.ViewModels.CampaignGroupWizard
{

    /// <summary>
    /// Viewmodel for the account tab of creating a new campaign.
    /// </summary>
    public sealed partial class CampaignGroupFormAllViewModel
    {

        /// <summary>
        /// Collection of all selected publishers this campaign group on which
        /// this campaign group should operate.
        /// </summary>
        [Required]
        public IEnumerable<Publisher> Publishers { get; set; }

        /// <summary>
        /// The guid of the account we select for this campaign.
        /// TODO This has to be selected for each publisher.
        /// </summary>
        [Required]
        public Guid AccountTaboolaId { get; set; }

        /// <summary>
        /// Name of this campaign group, human readable. This will be added onto
        /// all generated names for each campaign.
        /// </summary>
        [Required]
        public string CampaignNameSuffix { get; set; }

        /// <summary>
        /// Custom tracking code tags.
        /// </summary>
        public string UtmCustom { get; set; }

        /// <summary>
        /// The branding text for this campaign group.
        /// </summary>
        [Required]
        public string BrandingText { get; set; }

        /// <summary>
        /// Url to which this campaign group should go.
        /// </summary>
        [Required]
        public string Url { get; set; }

    }
}
