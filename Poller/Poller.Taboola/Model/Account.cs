using System;
using System.Runtime.Serialization;

namespace Poller.Taboola.Model
{
    [DataContract]
    internal class AccountDetails
    {
        [DataMember(Name = "partner_types")]
        public string[] PartnerTypes { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "campaign_types")]
        public string[] CampaignTypes { get; set; }
    }

    [DataContract]
    internal class Account
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "account_id")]
        public string AccountId { get; set; }

        [DataMember(Name = "partner_types")]
        public string[] PartnerTypes { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "Currency")]
        public string Currency { get; set; }

        [DataMember(Name = "campaign_types")]
        public string[] CampaignTypes { get; set; }

        public string Details { get; set; }
    }
}
