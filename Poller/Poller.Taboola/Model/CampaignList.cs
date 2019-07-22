using System.Collections.Generic;
using System.Runtime.Serialization;
using Poller.Model;

namespace Poller.Taboola.Model
{
    [DataContract]
    public class CampaignList
    {
        /// <summary>
        /// The items in this result.
        /// </summary>
        [DataMember(Name = "results")]
        public IEnumerable<Campaign> Items { get; set; }
    }
}
