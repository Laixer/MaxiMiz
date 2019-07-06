using System;
using System.Runtime.Serialization;

namespace MaxiMiz.Poller.Model.Response
{
    [DataContract]
    public class PublisherItem
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "publisher_item_id")]
        public long PublisherItemId { get; set; }

        [DataMember(Name = "campaign")]
        public long Campaign { get; set; }

        public string ContentUrl { get; set; }

        public string Url { get; set; }

        [DataMember(Name = "clicks")]
        public long Clicks { get; set; }

        [DataMember(Name = "impressions")]
        public long Impressions { get; set; }

        [DataMember(Name = "spent")]
        public decimal Spent { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "actions")]
        public long Actions { get; set; }
    }
}
