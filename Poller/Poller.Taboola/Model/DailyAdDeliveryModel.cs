using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Poller.Model.Data
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DailyAdDeliveryModel
    {
        [EnumMember(Value = "balanced")]
        Balanced,
        [EnumMember(Value = "accelerated")]
        Accelerated,
        [EnumMember(Value = "strict")]
        Strict
    }
}
