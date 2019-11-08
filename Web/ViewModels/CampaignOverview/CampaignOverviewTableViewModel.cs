using Maximiz.ViewModels.EntityModels;
using Maximiz.ViewModels.Utility;
using System.Collections.Generic;

namespace Maximiz.ViewModels.CampaignOverview
{

    /// <summary>
    /// Viewmodel for the partial view in the campaign overview.
    /// </summary>
    public sealed class CampaignOverviewTableViewModel : IncludeCurrencySymbolViewModel
    {

        /// <summary>
        /// Contains all campaigns that have to be displayed in the table.
        /// </summary>
        public IEnumerable<CampaignModel> CampaignList { get; set; }

    }
}
