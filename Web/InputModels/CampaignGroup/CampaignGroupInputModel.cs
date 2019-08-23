using Maximiz.Model.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Maximiz.InputModels
{
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
        [Required]
        [Display(Name = "Include Geo-targeting")]
        public int[] Location_Inc { get; set; }

        /// <summary>
        /// Geolocations to exclude.
        /// </summary>
        [Required]
        [Display(Name = "Exclude Geo-targeting")]
        public int[] Location_Exc { get; set; }

        /// <summary>
        /// Language of the campaign, 2 chars.
        /// </summary>
        public string[] Language { get; set; }

        /// <summary>
        /// Targeted devices.
        /// </summary>
        public Device[] Devices { get; set; }

        public SelectList DeviceSelectList =>  
            new SelectList(Enum.GetValues(typeof(Device)).OfType<Device>()
            .Select(x =>
                new SelectListItem
                {
                    Text = x.ToString(),
                    Value = x.GetEnumMemberName()
                }));

        /// <summary>
        /// Targeted operating systems.
        /// </summary>
        [Display(Name = "Operating Systems")]
        public OS[] OperatingSystems { get; set; }

        /// <summary>
        /// The initial CPC per item.
        /// </summary>
        [Required, Display(Name = "CPC")]
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
        public decimal? DailyBudget { get; set; }

        /// <summary>
        /// Delivery mode.
        /// </summary>
        [Required, Display(Name = "Ad Delivery")]
        public Delivery Delivery { get; set; }

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
        /// Status.
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Tracking code.
        /// </summary>
        [Required, Display(Name = "Tracking Code")]
        public string Utm { get; set; }

        /// <summary>
        /// Connections.
        /// </summary>
        public Connection[] Connections { get; set; }
    }
}
