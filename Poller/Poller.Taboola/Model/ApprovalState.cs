using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Poller.Taboola.Model
{
    /// <summary>
    /// Indicates the taboola state of approval
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ApprovalState
    {

        /// <summary>
        /// Has been approved by the Taboola API.
        /// </summary>
        [EnumMember(Value = "approved")]
        Approved,

        /// <summary>
        /// Has been submitted and is awaiting approval from the Taboola API.
        /// </summary>
        [EnumMember(Value = "pending")]
        Pending,

        /// <summary>
        /// Has been rejected by the Taboola API.
        /// </summary>
        [EnumMember(Value = "rejected")]
        Rejected
    }
}
