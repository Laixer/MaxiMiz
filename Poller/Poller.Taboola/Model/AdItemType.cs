using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Poller.Taboola.Model
{

    /// <summary>
    /// Represents the type of ad item. We do not use this at the moment.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    internal enum AdItemType
    {

        /// <summary>
        /// RSS type.
        /// </summary>
        [EnumMember(Value = "rss")]
        Rss,

        /// <summary>
        /// Regular ad item type.
        /// </summary>
        [EnumMember(Value = "item")]
        Item

    }
}
