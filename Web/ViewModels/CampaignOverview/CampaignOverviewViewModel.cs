using Maximiz.ViewModels.Columns;
using Maximiz.ViewModels.EntityModels;

namespace Maximiz.ViewModels.CampaignOverview
{

    /// <summary>
    /// A viewmodel used for our <see cref="CampaignModel"/> overview.
    /// </summary>
    public sealed class CampaignOverviewViewModel
    {

        /// <summary>
        /// Query input string.
        /// </summary>
        public string SearchQuery { get; set; }

        /// <summary>
        /// Represents the currently selected order in which the table is sorted.
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// Represents the currently selected column by which the table is sorted.
        /// </summary>
        public ColumnCampaignOverview Column { get; set; }

    }
}
