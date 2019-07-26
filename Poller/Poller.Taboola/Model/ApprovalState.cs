using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Poller.Taboola.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ApprovalState
    {
        [EnumMember(Value = "approved")]
        Approved,
        [EnumMember(Value = "pending")]
        Pending,
        [EnumMember(Value = "rejected")]
        Rejected,
    }
}
