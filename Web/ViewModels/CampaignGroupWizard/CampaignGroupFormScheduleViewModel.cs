using System;
using System.ComponentModel.DataAnnotations;

namespace Maximiz.ViewModels.NewCampaignGroup
{

    /// <summary>
    /// View model for the schedule form for creating new campaign groups.
    /// </summary>
    public sealed partial class CampaignGroupFormAllViewModel
    {

        /// <summary>
        /// The start date for each item in the campaign group.
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The end date for each item in the campaign group.
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// If set to true we have no <seealso cref="EndDate"/>.
        /// </summary>
        public bool IgnoreEndDate { get; set; }

    }
}
