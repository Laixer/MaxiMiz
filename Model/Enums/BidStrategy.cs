using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Maximiz.Model.Enums
{

    /// <summary>
    /// Used to indicate our bid strategy.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BidStrategy
    {
        [EnumMember(Value = "smart")]
        Smart,

        [EnumMember(Value = "fixed")]
        Fixed

    }
}
