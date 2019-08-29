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
        /// <summary>
        /// Used as a default if we don't know the state.
        /// </summary>
        [EnumMember(Value = "unknown")]
        Unknown,

        /// <summary>
        /// Used when the Backend requests the creation of
        /// an item in our own database. This is the state
        /// before the Poller has acutally created said item.
        /// </summary>
        [EnumMember(Value = "submitted")]
        Submitted,

        /// <summary>
        /// Item exists externally and is approved.
        /// </summary>
        [EnumMember(Value = "approved")]
        Approved,

        /// <summary>
        /// Item exists externally but has not yet been 
        /// approved.
        /// </summary>
        [EnumMember(Value = "pending")]
        Pending,

        /// <summary>
        /// Item has been rejected externally.
        /// </summary>
        [EnumMember(Value = "rejected")]
        Rejected,
    }
}
