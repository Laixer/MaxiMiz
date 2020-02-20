using Maximiz.ViewModels.EntityModels;
using Maximiz.ViewModels.Utility;
using System.Collections.Generic;

namespace Maximiz.ViewModels.AdGroupOverview
{

    /// <summary>
    /// Viewmodel for displaying rows of ad groups in a table.
    /// </summary>
    public sealed class AdGroupOverviewTableViewModel : IncludeCurrencySymbolViewModel
    {

        /// <summary>
        /// Contains all ad groups that have to be displayed in the table.
        /// </summary>
        public IEnumerable<AdGroupModel> AdGroupList { get; set; }

    }
}
