using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Poller.Taboola.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    internal enum CampaignStatus
    {
        [EnumMember(Value = "running")]
        Running,
        [EnumMember(Value = "paused")]
        Paused,
        [EnumMember(Value = "pending_start_date")]
        PendingStartDate,
        [EnumMember(Value = "depleted_monthly")]
        DepletedMonthly,
        [EnumMember(Value = "depleted")]
        Depleted,
        [EnumMember(Value = "expired")]
        Expired,
        [EnumMember(Value = "terminated")]
        Terminated,
        [EnumMember(Value = "frozen")]
        Frozen,
        [EnumMember(Value = "pending_approval")]
        PendingApproval,
        [EnumMember(Value = "rejected")]
        Rejected,
    }
}
