using System.Runtime.Serialization;

namespace Poller.Taboola.Model
{

    /// <summary>
    /// Mirrors the Taboola AdItem result we get
    /// from all ad item functions. This is coupled 
    /// with <see cref="AdItemReports"/>.
    /// </summary>
    /// <remarks>
    /// We skip type [ITEM, RSS] because it is not relevant for our specific
    /// implementation. </remarks>
    [DataContract]
    internal class AdItemMain
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
        /// The url for the thumbnail of the image.
        /// </summary>
        [DataMember(Name = "thumbnnail_url")]
        public string ThumbnailUrl { get; set; }

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
        public string ApprovalState { get; set; }

        /// <summary>
        /// Ad item status.
        /// </summary>
        [DataMember(Name = "status")]
        public string AdItemStatus { get; set; }

        /// <summary>
        /// Indicates the type of the ad item. We do not use this at this moment.
        /// The possibilities are RSS and ITEM.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

    }
}
