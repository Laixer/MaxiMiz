using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Poller.Taboola.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    internal enum BidType
    {
        [EnumMember(Value = "fixed")]
        Fixed,
        [EnumMember(Value = "optimized_conversions")]
        OptimizedConversions,
        [EnumMember(Value = "optimized_pageviews")]
        OptimizedPageviews,
    }
}
