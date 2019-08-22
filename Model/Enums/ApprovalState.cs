using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Maximiz.Model.Enums
{

    /// <summary>
    /// Model enum to represent our approval states.
    /// These will be consistent enough through our
    /// different advertisement providers.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ApprovalState
    {
        [EnumMember(Value = "unknown")]
        Unknown,
        [EnumMember(Value = "approved")]
        Approved,
        [EnumMember(Value = "pending")]
        Pending,
        [EnumMember(Value = "rejected")]
        Rejected,
    }
}
