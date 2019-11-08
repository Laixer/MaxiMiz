using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Maximiz.Model.Enums
{

    /// <summary>
    /// Model enum to represent an ad item status.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AdItemStatus
    {
        [EnumMember(Value = "unknown")]
        Unknown,
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
