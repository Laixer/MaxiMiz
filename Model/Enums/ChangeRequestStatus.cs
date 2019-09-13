using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Maximiz.Model.Enums
{

    /// <summary>
    /// Used to indicate the status of our requested changes. If a user changes
    /// some properties of an entity in our backend, these changes get pushed
    /// to our local database. After, the poller attempts to update these changes
    /// to the external advertiser API. These changes might be rejected or approved,
    /// which will be indicated by this enum.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ChangeRequestStatus
    {
        /// <summary>
        /// The user has changed something in our local database, and the change
        /// is requested to be pushed to the external advertiser API. This has
        /// not yet happened.
        /// 
        /// This is also the default value.
        /// </summary>
        [EnumMember(Value="requested")]
        Requested,

        /// <summary>
        /// Any requested changes have been approved and committed.
        /// </summary>
        [EnumMember(Value="up_to_date")]
        UpToDate,

        /// <summary>
        /// The last requested changes were denied by the external advertiser
        /// API. The changes were restored, based on what the API returned when
        /// requesting the current entity status.
        /// </summary>
        [EnumMember(Value = "denied_and_restored")]
        DeniedAndRestored
    }
}
