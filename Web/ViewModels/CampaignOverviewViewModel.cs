using Maximiz.ViewModels.EntityModels;
using System.Collections.Generic;

namespace Maximiz.ViewModels
{

    /// <summary>
    /// A viewmodel used for our <see cref="CampaignModel"/> overview.
    /// </summary>
    public sealed class CampaignOverviewViewModel
    {

        /// <summary>
        /// List of campaigns we get from the database.
        /// </summary>
        public IEnumerable<CampaignModel> Campaigns { get; set; }

        /// <summary>
        /// Query input string.
        /// </summary>
        public string SearchQuery { get; set; }

    }
}
