using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Maximiz.Model.Enums
{

    /// <summary>
    /// Model enum to represent our budget model.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BudgetModel
    {

        [EnumMember(Value = "campaign")]
        Campaign,

        /// <summary>
        /// Item exists externally and is approved.
        /// </summary>
        [EnumMember(Value = "monthly")]
        Monthly

    }
}
