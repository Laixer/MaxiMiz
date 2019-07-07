using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MaxiMiz.Poller.Model.Response
{
    [DataContract]
    public class Campaign
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "branding_text")]
        public string Branding { get; set; }

        [DataMember(Name = "cpc")]
        public decimal Cpc { get; set; }

        [DataMember(Name = "spending_limit")]
        public int SpendingLimit { get; set; }

        [DataMember(Name = "spending_limit_model")]
        public string SpendingLimitModel { get; set; }
        
        [DataMember(Name = "comments")]
        public string Note { get; set; }
    }
}
