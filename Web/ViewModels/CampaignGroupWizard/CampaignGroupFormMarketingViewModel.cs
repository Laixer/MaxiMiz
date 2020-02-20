using Maximiz.ViewModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace Maximiz.ViewModels.CampaignGroupWizard
{

    /// <summary>
    /// Viewmodel for the marketing tab in campaign group creation.
    /// </summary>
    public sealed partial class CampaignGroupFormAllViewModel
    {

        /// <summary>
        /// Use the auto pilot functionality or not.
        /// </summary>
        [Required]
        public bool Autopilot { get; set; }

        /// <summary>
        /// Indicates the initial cpc for each item in the campaign group.
        /// </summary>
        [Required]
        public decimal Cpc { get; set; }

        /// <summary>
        /// Bid strategy for each item in the campaign group.
        /// </summary>
        [Required]
        public BidStrategy BidStrategy { get; set; }

        /// <summary>
        /// Delivery for each item in the campaign group.
        /// </summary>
        [Required]
        public Delivery Delivery { get; set; }

        /// <summary>
        /// Total budget for the campaign group.
        /// </summary>
        public decimal Budget { get; set; } 

        /// <summary>
        /// The budget model for each item in the campaign group.
        /// </summary>
        [Required]
        public BudgetModel BudgetModel { get; set; }

        /// <summary>
        /// Daily budget, 0.0 means inifinite, but this is handled through our
        /// <see cref="BudgetDailyInfinite"/> property. 
        /// </summary>
        public decimal BudgetDaily { get; set; }

        /// <summary>
        /// Indicates if our daily budget is infinite.
        /// </summary>
        public bool BudgetDailyInfinite { get; set; }

    }
}
