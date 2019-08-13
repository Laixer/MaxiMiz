using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Poller.Helper;

[assembly: InternalsVisibleTo("Poller.Test")]
namespace Poller.Taboola.Model
{

    /// <summary>
    /// Mirrors the properties of a Taboola
    /// Account we get from their API.
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
        /// with underscores. Storing whitespaces in
        /// your database is a bad idea.
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

    }
}
