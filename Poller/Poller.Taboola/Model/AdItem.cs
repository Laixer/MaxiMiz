using System;
using System.Runtime.Serialization;

namespace Poller.Taboola.Model
{
    [DataContract]
    internal class AdItem
    {
        /// <summary>
        /// The id supplied by the remote network.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The id issued by the publisher, this is issued as a string by the publisher
        /// but we want an int so we need to supply a setter.
        /// </summary>
        [DataMember(Name = "item")]
        public string Item { set => Id = value; }

        /// <summary>
        /// The ad title.
        /// </summary>
        [DataMember(Name = "item_name")]
        public string ItemName { set => Title = value; }

        /// <summary>
        /// The id of the campaign this item belongs to.
        /// </summary>
        [DataMember(Name = "campaign_id")]
        public long CampaignId { set => Campaign = value; }

        /// <summary>
        /// The id of the campaign this item belongs to.
        /// </summary>
        [DataMember(Name = "campaign")]
        public long Campaign { get; set; }

        /// <summary>
        /// The name the campaign this item belongs to.
        /// </summary>
        [DataMember(Name = "campaign_name")]
        public string CampaignName { get; set; }

        /// <summary>
        /// The url for the thumbnail of this item.
        /// </summary>
        [DataMember(Name = "thumbnail_url")]
        public string Content { get; set; }

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
        /// The amount of clicks on this item.
        /// </summary>
        [DataMember(Name = "clicks")]
        public long Clicks { get; set; }

        /// <summary>
        /// The amount of impressions on this item.
        /// </summary>
        [DataMember(Name = "impressions")]
        public long Impressions { get; set; }

        /// <summary>
        /// Cost per click.
        /// </summary>
        [DataMember(Name = "cpc")]
        public decimal Cpc { get; set; }

        /// <summary>
        /// The amount spent on this item.
        /// </summary>
        [DataMember(Name = "spent")]
        public decimal Spent { get; set; }

        /// <summary>
        /// The currency in which all monetary units are calculated.
        /// </summary>
        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        /// <summary>
        /// The amount of actions on the item.
        /// </summary>
        [DataMember(Name = "actions")]
        public long Actions { get; set; }

        /// <summary>
        /// Whether the item is still active.
        /// </summary>
        [DataMember(Name = "is_active")]
        public bool Active { get; set; }

        /// <summary>
        /// Ad item status.
        /// </summary>
        [DataMember(Name = "status")]
        public string StatusText { get; set; }
    }
}
