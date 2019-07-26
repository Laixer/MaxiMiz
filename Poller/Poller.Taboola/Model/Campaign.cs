using System;
using System.Runtime.Serialization;
using Poller.Extensions;
using Poller.Model.Data;

namespace Poller.Taboola.Model
{
    // TODO:
    // - activity_schedule
    // - verification_pixel
    // - marketing_objective
    // - publisher_bid_modifier
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
        public DailyAdDeliveryModel Delivery { get; set; }
        public string DeliveryText
        {
            get
            {
                switch (Delivery)
                {
                    case DailyAdDeliveryModel.Balanced:
                        return "balaned"; // TODO: This is a typo
                    case DailyAdDeliveryModel.Accelerated:
                        return "accelerated";
                    case DailyAdDeliveryModel.Strict:
                        return "strict";
                }

                throw new Exception();
            }
        }

        [DataMember(Name = "spending_limit")]
        public decimal SpendingLimit { get; set; }

        [DataMember(Name = "spending_limit_model")]
        public SpendingLimitModel SpendingLimitModel { get; set; }
        public string SpendingLimitModelText
        {
            get
            {
                switch (SpendingLimitModel)
                {
                    case SpendingLimitModel.Monthly:
                        return "monthly";
                    case SpendingLimitModel.Entire:
                        return "campaign";
                }

                throw new Exception();
            }
        }

        [DataMember(Name = "country_targeting")]
        public Target CountryTargeting { get; set; }

        [DataMember(Name = "sub_country_targeting")]
        public Target SubCountryTargeting { get; set; }

        [DataMember(Name = "postal_code_targeting")]
        public Target PostalCodeTargeting { get; set; }

        [DataMember(Name = "contextual_targeting")]
        public Target ContextualTargeting { get; set; }

        [DataMember(Name = "platform_targeting")]
        public Target PlatformTargeting { get; set; }

        [DataMember(Name = "publisher_targeting")]
        public Target PublisherTargeting { get; set; }

        [DataMember(Name = "os_targeting")]
        public Target OsTargeting { get; set; }

        [DataMember(Name = "connection_type_targeting")]
        public Target ConnectionTypeTargeting { get; set; }

        //[DataMember(Name = "audience_segments_multi_targeting")]
        //public Target AudienceSegmentsMultiTargeting { get; set; }

        //[DataMember(Name = "custom_audience_targeting")]
        //public Target CustomAudienceTargeting { get; set; }

        //[DataMember(Name = "marking_label_multi_targeting")]
        //public Target MarkingLabelMultiTargeting { get; set; }

        [DataMember(Name = "cpa_goal")]
        public decimal CpaGoal { get; set; }

        [DataMember(Name = "comments")]
        public string Note { get; set; }

        [DataMember(Name = "spent")]
        public decimal Spent { get; set; }

        [DataMember(Name = "bid_type")]
        public BidType BidStrategy { get; set; }

        [DataMember(Name = "traffic_allocation_mode")]
        public TrafficAllocationMode TrafficAllocationMode { get; set; }

        [DataMember(Name = "start_date")]
        public DateTime? StartDate { get; set; }

        [DataMember(Name = "end_date")]
        public DateTime? EndDate { get; set; }

        [DataMember(Name = "approval_state")]
        public ApprovalState ApprovalState { get; set; }
        public string ApprovalStateText { get => ApprovalState.GetEnumMemberName(); }

        [DataMember(Name = "status")]
        public CampaignStatus Status { get; set; }

        [DataMember(Name = "is_active")]
        public bool Active { get; set; }
    }
}
