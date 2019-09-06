using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Models
{

    /// <summary>
    /// A Model for overview view from campaign
    /// </summary>
    public class OverviewCampaignModel
    {

        /// <summary>
        /// List of campaigns we get from the database.
        /// </summary>
        public IEnumerable<Campaign> Campaigns { get; set; }

        /// <summary>
        /// Query input.
        /// </summary>
        public string Query { get; set; }

    }
}
