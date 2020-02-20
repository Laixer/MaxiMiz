using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Maximiz.Model.Enums
{

    /// <summary>
    /// Model enum to represent a delivery mode.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Delivery
    {
        [EnumMember(Value = "balanced")]
        Balanced,
        [EnumMember(Value = "accelerated")]
        Accelerated,
        [EnumMember(Value = "strict")]
        Strict
    }
}
