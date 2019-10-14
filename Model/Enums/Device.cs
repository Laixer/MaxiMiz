using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Maximiz.Model.Enums
{

    /// <summary>
    /// Model enum to represent a device type.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Device
    {
        [EnumMember(Value = "mobile")]
        Mobile,

        [EnumMember(Value = "tablet")]
        Tablet,

        [EnumMember(Value = "laptop")]
        Laptop,

        [EnumMember(Value = "desktop")]
        Desktop,

        [EnumMember(Value = "wearable")]
        Wearable
    }
}
