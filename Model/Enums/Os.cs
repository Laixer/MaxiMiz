using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Maximiz.Model.Enums
{

    /// <summary>
    /// Model enum to represent an operating syste.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OS
    {
        [EnumMember(Value = "windows")]
        Windows,

        [EnumMember(Value = "linux")]
        Linux,

        [EnumMember(Value = "osx")]
        OSX,

        [EnumMember(Value = "android")]
        Android,

        [EnumMember(Value = "ios")]
        iOS,

        [EnumMember(Value = "unix")]
        Unix,

        [EnumMember(Value = "chromeos")]
        Chromeos
    }
}
