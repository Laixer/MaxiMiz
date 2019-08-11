using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Poller.Helper;

[assembly: InternalsVisibleTo("Poller.Test")]
namespace Poller.Taboola.Model
{

    /// <summary>
    /// Represents a Taboola account.
    /// </summary>
    [DataContract]
    internal class Account
    {
        /// <summary>
        /// Taboola internal id, being an integer.
        /// TODO Is this correct?
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// User created name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// User created name, whitespaces replaced 
        /// with underscores.
        /// </summary>
        [DataMember(Name = "account_id")]
        public string AccountId { get; set; }

        /// <summary>
        /// List of partner types. Options:
        /// - PUBLISHER
        /// - ADVERTISER
        /// TODO Is this correct?
        /// </summary>
        [DataMember(Name = "partner_types")]
        public string[] PartnerTypes { get; set; }

        /// <summary>
        /// Account type.
        /// TODO What does this mean?
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Currency string.
        /// </summary>
        [DataMember(Name = "Currency")]
        public string Currency { get; set; }

        /// <summary>
        /// List of types of campaigns belonging to 
        /// this account.
        /// TODO What does this mean?
        /// </summary>
        [DataMember(Name = "campaign_types")]
        public string[] CampaignTypes { get; set; }

        /// <summary>
        /// JSON string containing unused data which
        /// we do have to store.
        /// </summary>
        public string Details
        {
            // FUTURE: Improve
            get => Json.Serialize(new AccountDetails
            {
                PartnerTypes = PartnerTypes?.Select(s => s.ToLowerInvariant()).ToArray(),
                Type = Type.ToLower(),
                CampaignTypes = CampaignTypes?.Select(s => s.ToLowerInvariant()).ToArray(),
            });
        }
    }

    /// <summary>
    /// Represents the properties of our 
    /// Taboola account which we do not
    /// store explicitly in our own database.
    /// </summary>
    [DataContract]
    internal class AccountDetails
    {

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "partner_types")]
        public string[] PartnerTypes { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "campaign_types")]
        public string[] CampaignTypes { get; set; }

    }
}
