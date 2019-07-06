using System;
using System.Runtime.Serialization;

namespace MaxiMiz.Poller.Model.Response
{
    [DataContract]
    public class PublisherItem
    {
        /// <summary>
        /// The primary key for the database.
        /// </summary>
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The id issued by the publisher, this is issued as a string by the publisher but we want an int so we need to supply a setter.
        /// </summary>
        [DataMember(Name = "item")]
        public string Item { set => PublisherItemId = long.Parse(value); }

        /// <summary>
        /// The id supplied by the publisher.
        /// </summary>
        public long PublisherItemId { get; set; }

        /// <summary>
        /// The id of the campaign this item belongs to
        /// </summary>
        [DataMember(Name = "campaign")]
        public long Campaign { get; set; }

        /// <summary>
        /// The url for the thumbnail of this item.
        /// </summary>
        [DataMember(Name = "thumbnail_url")]
        public string ContentUrl { get; set; }

        /// <summary>
        /// The url where the item leads to.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

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
    }
}
