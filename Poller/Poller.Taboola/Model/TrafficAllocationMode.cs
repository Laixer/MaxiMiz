using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Poller.Taboola.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    internal enum TrafficAllocationMode
    {
        [EnumMember(Value = "optimized")]
        Optimized,
        [EnumMember(Value = "even")]
        Even,
    }
}
