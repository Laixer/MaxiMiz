using Maximiz.ViewModels.Columns;

namespace Maximiz.ViewModels.AdGroupOverview
{

    /// <summary>
    /// Viewmodel for displayingn ad groups in an overview.
    /// </summary>
    public sealed class AdGroupOverviewViewModel
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
        public ColumnAdGroupOverview Column { get; set; }

    }
}
