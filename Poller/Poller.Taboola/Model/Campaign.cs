using System;
using System.Runtime.Serialization;

namespace Poller.Taboola.Model
{
    internal enum AdDelivery
    {
        Balaned,
        Accelerated,
        Strict
    }

    [DataContract]
    internal class Campaign
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "advertiser_id")]
        public string Account { get; set; }

        [DataMember(Name = "branding_text")]
        public string Branding { get; set; }

        [DataMember(Name = "tracking_code")]
        public string Utm { get; set; }

        [DataMember(Name = "cpc")]
        public decimal Cpc { get; set; }

        [DataMember(Name = "daily_cap")]
        public decimal? DailyCap { get; set; }

        [DataMember(Name = "daily_ad_delivery_model")]
        public string Delivery { get; set; }

        public AdDelivery Delivery2 { get; set; }


        [DataMember(Name = "spending_limit")]
        public decimal SpendingLimit { get; set; }

        [DataMember(Name = "spending_limit_model")]
        public string SpendingLimitModel { get; set; }

        [DataMember(Name = "cpa_goal")]
        public decimal CpaGoal { get; set; }

        [DataMember(Name = "comments")]
        public string Note { get; set; }

        [DataMember(Name = "spent")]
        public decimal Spent { get; set; }

        [DataMember(Name = "bid_type")]
        public string BidStrategy { get; set; }

        [DataMember(Name = "traffic_allocation_mode")]
        public string TrafficAllocationMode { get; set; }

        [DataMember(Name = "start_date")]
        public DateTime? StartDate { get; set; }

        [DataMember(Name = "end_date")]
        public DateTime? EndDate { get; set; }

        [DataMember(Name = "approval_state")]
        public string ApprovalStatus { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "is_active")]
        public bool Active { get; set; }
    }
}
