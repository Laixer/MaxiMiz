using System.Runtime.Serialization;

namespace Poller.Model.Data
{
    public enum AdItemStatus
    {
        [EnumMember(Value = "running")]
        Running,
        [EnumMember(Value = "paused")]
        Paused,
        [EnumMember(Value = "stopped")]
        Stopped,
        [EnumMember(Value = "pending")]
        Pending,
        [EnumMember(Value = "rejected")]
        Rejected,
        [EnumMember(Value = "unknown")]
        Unknown,
    }
}
