using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Poller.Taboola.Model
{

    /// <summary>
    /// Enum to indicate the way we should interpret
    /// the value string array.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TargetType
    {

        /// <summary>
        /// All are included.
        /// </summary>
        [EnumMember(Value = "include")]
        Include,

        /// <summary>
        /// All are excluded.
        /// </summary>
        [EnumMember(Value = "exclude")]
        Exclude,

        /// <summary>
        /// All.
        /// </summary>
        [EnumMember(Value = "all")]
        All
    }

}
