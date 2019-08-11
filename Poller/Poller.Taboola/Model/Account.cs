using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Poller.Helper;

[assembly: InternalsVisibleTo("Poller.Test")]
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
        public string Id { get; set; }

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
}
