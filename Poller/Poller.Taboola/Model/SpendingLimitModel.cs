using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Poller.Taboola.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    internal enum SpendingLimitModel
    {
        [EnumMember(Value = "monthly")]
        Monthly,
        [EnumMember(Value = "entire")]
        Entire,
    }
}
