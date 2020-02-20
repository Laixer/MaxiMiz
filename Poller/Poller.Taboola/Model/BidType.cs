using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Poller.Taboola.Model
{

    /// <summary>
    /// Indicates how we want to modify our cpc bid.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    internal enum BidType
    {
        /// <summary>
        /// Constant bid.
        /// </summary>
        [EnumMember(Value = "fixed")]
        Fixed,

        /// <summary>
        /// Bid optimized for maximum conversion.
        /// </summary>
        [EnumMember(Value = "optimized_conversions")]
        OptimizedConversions,

        /// <summary>
        /// Bid optimized for maximum pageviews.
        /// </summary>
        [EnumMember(Value = "optimized_pageviews")]
        OptimizedPageviews
    }
}
