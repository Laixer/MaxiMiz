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

        /// <summary>
        /// The specified budget holds for the entire campaign.
        /// </summary>
        [EnumMember(Value = "campaign")]
        Campaign,

        /// <summary>
        /// The specified budget holds for each month.
        /// </summary>
        [EnumMember(Value = "monthly")]
        Monthly

    }
}
