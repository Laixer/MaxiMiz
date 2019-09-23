using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Maximiz.Model.Enums
{

    /// <summary>
    /// Model enum to represent an ad item status.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
        [EnumMember(Value = "unknown")]
        Unknown,
        [EnumMember(Value = "running")]
        Running,
        [EnumMember(Value = "crawling")]
        Crawling,
        [EnumMember(Value = "crawling_error")]
        CrawlingError,
        [EnumMember(Value = "need_to_edit")]
        NeedToEdit,
        [EnumMember(Value = "paused")]
        Paused,
        [EnumMember(Value = "stopped")]
        Stopped,
        [EnumMember(Value = "pending_approval")]
        PendingApproval,
        [EnumMember(Value = "rejected")]
        Rejected
    }
}
