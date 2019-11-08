using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Maximiz.ViewModels.Enums
{

    /// <summary>
    /// Enum to represent our countries.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Location
    {
        [EnumMember(Value = "nl")]
        NL,
        [EnumMember(Value = "uk")]
        UK,
        [EnumMember(Value = "es")]
        ES,
        [EnumMember(Value = "de")]
        DE,
        [EnumMember(Value = "fr")]
        FR
    }

}
