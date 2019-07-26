using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Poller.Taboola.Model
{
    [DataContract]
    internal class Target
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum TargetType
        {
            [EnumMember(Value = "include")]
            Include,
            [EnumMember(Value = "exclude")]
            Exclude,
            [EnumMember(Value = "all")]
            All,
        }

        [DataMember(Name = "type")]
        public TargetType Type { get; set; }

        [DataMember(Name = "value")]
        public string[] Value { get; set; }

        [DataMember(Name = "href")]
        public string Href { get; set; }
    }
}
