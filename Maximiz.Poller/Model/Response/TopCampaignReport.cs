using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MaxiMiz.Poller.Model.Response
{
    [DataContract]
    public class TopCampaignReport
    {
        [DataMember(Name = "last-used-rawdata-update-time-gmt-millisec")]
        internal readonly ulong lastUpdateTimeEpochGmtInMillis;

        [DataMember(Name = "results")]
        internal readonly List<PublisherItem> items;
    }
}
