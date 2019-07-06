using System;
using System.Runtime.Serialization;

namespace MaxiMiz.Poller.Model.Response
{
    [DataContract]
    internal class PublisherItem
    {
        [DataMember(Name = "id")]
        internal readonly Guid id;

        [DataMember(Name = "publisher_item_id")]
        internal readonly ulong publisherItemId;

        [DataMember(Name = "clicks")]
        internal readonly ulong clicks;

        [DataMember(Name = "impressions")]
        internal readonly ulong impressions;

        [DataMember(Name = "spent")]
        internal readonly decimal spent;

        [DataMember(Name = "currency")]
        internal readonly string currency;

        [DataMember(Name = "actions")]
        internal ulong actions;
    }
}
