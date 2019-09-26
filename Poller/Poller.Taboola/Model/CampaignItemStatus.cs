using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Poller.Taboola.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    internal enum CampaignItemStatus
    {
        [EnumMember(Value = "running")]
        Running,
        [EnumMember(Value = "crawling")]
        Crawling,
        [EnumMember(Value = "crawling_error")]
        CrawlingError,
        [EnumMember(Value = "need_to_edit")]
        NeedToEdit,
        [EnumMember(Value = "paused")]
        Paused,
        [EnumMember(Value = "stopped")]
        Stopped,
        [EnumMember(Value = "pending_approval")]
        PendingApproval,
        [EnumMember(Value = "rejected")]
        Rejected
    }
}
