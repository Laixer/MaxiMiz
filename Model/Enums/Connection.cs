using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Maximiz.Model.Enums
{

    /// <summary>
    /// Model enum to represent a connection.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Connection
    {
        [EnumMember(Value = "cable")]
        Cable,
        [EnumMember(Value = "wifi")]
        Wifi,
        [EnumMember(Value = "cellular")]
        Cellular
    }
}
