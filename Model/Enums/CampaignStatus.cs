using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Maximiz.Model.Enums
{

    /// <summary>
    /// Model enum to represent a campaign status.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CampaignStatus
    {
        /// <summary>
        /// Default value.
        /// </summary>
        [EnumMember(Value = "unknown")]
        Unknown,

        /// <summary>
        /// Campaign is active and running.
        /// </summary>
        [EnumMember(Value = "running")]
        Running,

        /// <summary>
        /// Campaign was paused by the user.
        /// </summary>
        [EnumMember(Value = "paused")]
        Paused,

        /// <summary>
        /// Campaign has run out of budget.
        /// </summary>
        [EnumMember(Value = "depleted")]
        Depleted,

        /// <summary>
        /// Campaign has run out of budget for this month.
        /// </summary>
        [EnumMember(Value = "depleted_monthly")]
        DepletedMonthly,

        /// <summary>
        /// Campaign end date has passed.
        /// </summary>
        [EnumMember(Value = "expired")]
        Expired,

        /// <summary>
        /// Campaign was stopped by the user.
        /// </summary>
        [EnumMember(Value = "terminated")]
        Terminated,

        /// <summary>
        /// Campaign was paused by the network for some reason.
        /// </summary>
        [EnumMember(Value = "frozen")]
        Frozen,

        /// <summary>
        /// Campaign has to be approved by the network.
        /// </summary>
        [EnumMember(Value = "pending_approval")]
        PendingApproval,

        /// <summary>
        /// Campaign was rejected by the network.
        /// </summary>
        [EnumMember(Value = "rejected")]
        Rejected,

        /// <summary>
        /// Campaign will be active once the start date is upon us.
        /// </summary>
        [EnumMember(Value = "pending_start_date")]
        PendingStartDate
    }
}
