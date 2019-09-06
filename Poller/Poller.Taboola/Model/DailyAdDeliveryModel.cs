using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Poller.Model.Data
{

    /// <summary>
    /// This indicates the way we want to deliver our ads on a daily basis.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DailyAdDeliveryModel
    {
        /// <summary>
        /// Evenly distributed throughout the day.
        /// </summary>
        [EnumMember(Value = "balanced")]
        Balanced,
        
        /// <summary>
        /// Ignoring daily cap.
        /// </summary>
        [EnumMember(Value = "accelerated")]
        Accelerated,

        /// <summary>
        /// Following daily cap.
        /// </summary>
        [EnumMember(Value = "strict")]
        Strict
    }
}
