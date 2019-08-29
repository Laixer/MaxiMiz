using System;
using System.Runtime.Serialization;
using Poller.Extensions;
using Poller.Model.Data;

namespace Poller.Taboola.Model
{

    /// <summary>
    /// Mirrors the Taboola AdItem result we get
    /// from all ad item functions. This is coupled 
    /// with <see cref="AdItemCoResult"/>.
    /// </summary>
    [DataContract]
    internal class AdItem
    {
        /// <summary>
        /// The id supplied by the remote network.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Unique numeric ID of the items parent campaign
        /// returned as a string.
        /// </summary>
        [DataMember(Name = "campaign_id")]
        public string CampaignId { get; set; }

        /// <summary>
        /// The url where the item leads to.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// The ad title.
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Whether the item is still active.
        /// </summary>
        [DataMember(Name = "is_active")]
        public bool Active { get; set; }

        /// <summary>
        /// Item approval status.
        /// </summary>
        [DataMember(Name = "approval_state")]
        public ApprovalState ApprovalState { get; set; }


        /// <summary>
        /// Campaign item status.
        /// </summary>
        [DataMember(Name = "status")]
        public CampaignItemStatus CampaignItemStatus { get; set; }

    }
}
