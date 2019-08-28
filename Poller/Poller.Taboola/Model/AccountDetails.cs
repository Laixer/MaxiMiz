using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Poller.Taboola.Model
{

    /// <summary>
    /// Represents the properties of our Taboola 
    /// account which we do not explicitly store 
    /// in our own database. This is used as a 
    /// template for our JSON converter.
    /// </summary>
    [DataContract]
    internal class AccountDetails
    {

        /// <summary>
        /// The internal Taboola database id.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The partner types.
        /// </summary>
        [DataMember(Name = "partner_types")]
        public string[] PartnerTypes { get; set; }

        /// <summary>
        /// The type of our account.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// The type of campaigns that are linked to
        /// this account.
        /// </summary>
        [DataMember(Name = "campaign_types")]
        public string[] CampaignTypes { get; set; }

        /// <summary>
        /// The human readable variant of our account name.
        /// </summary>
        [DataMember(Name = "name_human_readable")]
        public string NameHumanReadable { get; set; }

    }
}
