using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Maximiz.Model.Enums
{

    /// <summary>
    /// Model enum to represent a Bid Strategy.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BidStrategy
    {
        [EnumMember(Value = "smart")]
        Smart,
        [EnumMember(Value = "fixed")]
        Fixed
    }
}
