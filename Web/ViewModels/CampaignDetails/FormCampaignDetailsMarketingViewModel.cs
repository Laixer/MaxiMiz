using Maximiz.ViewModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace Maximiz.ViewModels.CampaignDetails
{

    /// <summary>
    /// Viewmodel to modify our marketing properties for a given campaign.
    /// </summary>
    public sealed partial class FormCampaignDetailsViewModel
    {

        // <summary>
        /// Use the auto pilot functionality or not.
        /// </summary>
        [Required]
        public bool Autopilot { get; set; }

        // <summary>
        /// Whether or not we want to have this campaign active.
        /// </summary>
        [Required]
        public bool Active { get; set; }

        /// <summary>
        /// User input cpc.
        /// </summary>
        [Required]
        public decimal Cpc { get; set; }

        /// <summary>
        /// User input bid strategy.
        /// </summary>
        [Required]
        public BidStrategy BidStrategy { get; set; }

        /// <summary>
        /// User input delivery.
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
