using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Maximiz.Model.Enums
{

    /// <summary>
    /// Model enum to represent the budget model of a campaign.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BudgetModel
    {
        [EnumMember(Value = "campaign")]
        Campaign,
        [EnumMember(Value = "monthly")]
        Monthly
    }
}