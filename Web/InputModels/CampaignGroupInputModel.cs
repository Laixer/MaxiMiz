using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Maximiz.InputModels
{
    /// <summary>
    /// A model containing the user based input for a single <see cref="Campaign"></see> entity.
    /// </summary>
    public class CampaignGroupInputModel
    {
        /// <summary>
        /// Campaign name.
        /// </summary>
        [Required(ErrorMessage = "A campaign name is required.")]
        [Display(Name = "Campaign Name")]
        [MaxLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// Campaign branding text.
        /// </summary>
        [Required]
        [Display(Name = "Branding")]
        [MaxLength(256)]
        public string BrandingText { get; set; }

        /// <summary>
        /// Geolocations to include.
        /// </summary>
        [Display(Name = "Include Geo-targeting")]
        public int[] LocationInclude { get; set; }

        /// <summary>
        /// Geolocations to exclude.
        /// </summary>
        [Display(Name = "Exclude Geo-targeting")]
        public int[] LocationExclude { get; set; }

        /// <summary>
        /// Language of the campaign, 2 chars.
        /// </summary>
        [MaxLength(2)]
        public string Language { get; set; }

        /// <summary>
        /// Targeted devices.
        /// </summary>
        [Required]
        [MinLength(1)]
        [Display(Name = "Targeted Devices")]
        public Device[] Devices { get; set; }

        /// <summary>
        /// Targeted operating systems.
        /// </summary>
        [Required]
        [MinLength(1)]
        [Display(Name = "Operating Systems")]
        public OS[] OperatingSystems { get; set; }

        /// <summary>
        /// The initial CPC per item.
        /// </summary>
        [Required]
        [Display(Name = "CPC")]
        public decimal InitialCpc { get; set; }

        /// <summary>
        /// Budget per campaign or period.
        /// </summary>
        [Required]
        [Display(Name = "Campaign Budget")]
        public decimal Budget { get; set; }

        /// <summary>
        /// Budget per day. Can be null.
        /// </summary>
        [Display(Name = "Daily Budget")]
        public decimal? DailyBudget { get; set; }

        /// <summary>
        /// Budget Model.
        /// </summary>
        [Required]
        [Display(Name = "Budget Model")]
        public BudgetModel BudgetModel { get; set; }

        /// <summary>
        /// Delivery mode.
        /// </summary>
        [Required]
        [Display(Name = "Ad Delivery")]
        public Delivery Delivery { get; set; }

        /// <summary>
        /// Bid strategy.
        /// </summary>
        [Required]
        [Display(Name = "Bid Strategy")]
        public BidStrategy BidStrategy { get; set; }

        /// <summary>
        /// Campaign start date.
        /// </summary>
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Campaign end date.
        /// </summary>
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Tracking code.
        /// </summary>
        [Required]
        [Display(Name = "Tracking Code")]
        public string Utm { get; set; }

        /// <summary>
        /// Supported connections.
        /// </summary>
        //public Connection[] Connections { get; set; }

    }
}
