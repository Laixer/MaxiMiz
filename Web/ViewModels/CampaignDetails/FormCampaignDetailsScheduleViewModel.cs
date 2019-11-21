using System;
using System.ComponentModel.DataAnnotations;

namespace Maximiz.ViewModels.CampaignDetails
{

    /// <summary>
    /// Partial viewmodel for our schedule properties for a given campaign.
    /// </summary>
    public sealed partial class FormCampaignDetailsViewModel
    {

        /// <summary>
        /// The start date for each item in the campaign group.
        /// TODO Not required at the moment, we should reimplement this later!
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The end date for each item in the campaign group.
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// If set to true we have no <seealso cref="EndDate"/>.
        /// </summary>
        public bool IgnoreEndDate { get; set; }

    }
}
