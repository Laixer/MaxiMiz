using System.ComponentModel.DataAnnotations;

namespace Maximiz.InputModels.Campaign
{
    //TODO: Check and determine needed properties and types
    public class CreateCampaignModel
    {
        public string Account { get; set; }

        [Required(ErrorMessage = "A campaign name is required.")]
        [Display(Name = "Campaign Name")]
        [MaxLength(128)]
        public string Name { get; set; }

        [Display(Name = "Branding")]
        public string BrandingText { get; set; }

        public string Url { get; set; }

        public string Objective { get; set; }
        //public string CTA { get; set; } ???
        public decimal SpendingLimit { get; set; }

        public string AdDelivery { get; set; }
        public decimal Cpc { get; set; }
        //TODO: Adjust CPC for: public string AdjustCPC { get; set; }

        public string GeoTargeting { get; set; }
        public string Device { get; set; }
        public string OperatingSystem { get; set; }
    }
}
