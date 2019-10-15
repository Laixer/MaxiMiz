using Maximiz.Model.Entity;
using System.Collections.Generic;

namespace Maximiz.Models
{

    /// <summary>
    /// A viewmodel used with the campaign overview.
    /// TODO Moet viewmodel worden
    /// </summary>
    public class OverviewCampaignModel
    {

        /// <summary>
        /// List of campaigns we get from the database.
        /// </summary>
        public IEnumerable<Campaign> Campaigns { get; set; }

        /// <summary>
        /// Query input string.
        /// </summary>
        public string Query { get; set; }

    }
}
