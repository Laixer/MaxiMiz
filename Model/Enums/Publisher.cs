using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Maximiz.Model.Enums
{

    /// <summary>
    /// Used to contain our publishers.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Publisher
    {
        [EnumMember(Value = "unknown")]
        Unknown,
        [EnumMember(Value = "google")]
        Google,
        [EnumMember(Value = "taboola")]
        Taboola,
        [EnumMember(Value = "outbrain")]
        Outbrain,
        [EnumMember(Value = "adroll")]
        Adroll,
        [EnumMember(Value = "criteo")]
        Criteo
    }
}
