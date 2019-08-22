using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Poller.Taboola.Model
{

    /// <summary>
    /// Object used by Taboola to indicate which
    /// items should be targeted and which items
    /// should not be targeted. Items can be any
    /// country, OS, etc.
    /// </summary>
    [DataContract]
    internal class Target
    {

        /// <summary>
        /// Enum to indicate the way we should interpret
        /// the value string array.
        /// </summary>
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

        /// <summary>
        /// The value string array can either be
        /// include, exclude or all.
        /// </summary>
        [DataMember(Name = "type")]
        public TargetType Type { get; set; }

        /// <summary>
        /// The values, this has to be mapped in
        /// some way.
        /// </summary>
        [DataMember(Name = "value")]
        public string[] Value { get; set; }

        /// <summary>
        /// If the value list is too long, this link
        /// contains an API call to retrieve all our
        /// values.
        /// </summary>
        [DataMember(Name = "href")]
        public string Href { get; set; }
    }
}
