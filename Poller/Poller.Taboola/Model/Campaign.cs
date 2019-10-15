using System;
using System.Runtime.Serialization;
using Poller.Extensions;
using Poller.Model.Data;

namespace Poller.Taboola.Model
{
    // TODO:
    // - activity_schedule
    // - verification_pixel --> doen we niets me

    /// <summary>
    /// Represents all Taboola API parameters for
    /// a single campaign.
    /// </summary>
    [DataContract]
    internal class Campaign
    {

        /// <summary>
        /// Unique numeric Id used by Taboola, returned
        /// as a string by the API.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The name the user gave to this account. A
        /// human readable name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The account_id of the campaign's advertiser
        /// account.
        /// </summary>
        [DataMember(Name = "advertiser_id")]
        public string Account { get; set; }

        /// <summary>
        /// The text that will appear below the title
        /// of each of our campaign items.
        /// </summary>
        [DataMember(Name = "branding_text")]
        public string BrandingText { get; set; }

        /// <summary>
        /// Query-string parameter added to the URL of
        /// the campaign items to allow for tracking.
        /// </summary>
        [DataMember(Name = "tracking_code")]
        public string Utm { get; set; }

        /// <summary>
        /// Cost per click.
        /// </summary>
        [DataMember(Name = "cpc")]
        public decimal Cpc { get; set; }

        /// <summary>
        /// Daily max spending limit.
        /// </summary>
        [DataMember(Name = "daily_cap")]
        public decimal? DailyCap { get; set; }

        /// <summary>
        /// Defines the way our campaign will be delivered on a daily basis. 
        /// This is stored as a string because our internal model uses lower
        /// case, but Taboola uses upper case.
        /// </summary>
        [DataMember(Name = "daily_ad_delivery_model")]
        public string DailyAdDeliveryModel { get; set; }

        /// <summary>
        /// An object representing the wanted publisher
        /// bid modifiers for this campaign.
        /// </summary>
        [DataMember(Name="publisher_bid_modifier")]
        public PublisherBidModifier PublisherBidModifier { get; set; }

        /// <summary>
        /// The maximum amount of money this campaign
        /// can consume.
        /// </summary>
        [DataMember(Name = "spending_limit")]
        public decimal SpendingLimit { get; set; }

        /// <summary>
        /// Determines the type of spending limit.
        /// </summary>
        [DataMember(Name = "spending_limit_model")]
        public string SpendingLimitModel { get; set; }

        /// <summary>
        /// Lists all countries the campaign will
        /// include or exclude.
        /// </summary>
        [DataMember(Name = "country_targeting")]
        public TargetDefault CountryTargeting { get; set; }

        /// <summary>
        /// Lists all regions or DMAs the campaign 
        /// will include or exclude.
        /// </summary>
        [DataMember(Name = "sub_country_targeting")]
        public TargetDefault SubCountryTargeting { get; set; }

        /// <summary>
        /// Lists all postal codes the campaign will
        /// include or exclude. If NULL, this will
        /// indicate we target all postal codes.
        /// </summary>
        [DataMember(Name = "postal_code_targeting")]
        public TargetDefault PostalCodeTargeting { get; set; }

        /// <summary>
        /// Lists all contexts (?) the campaign will
        /// include or exclude. This only shows up 
        /// in the DELETE CAMPAIGN operation.
        /// TODO How to interpret this?
        /// </summary>
        [DataMember(Name = "contextual_targeting")]
        public TargetDefault ContextualTargeting { get; set; }

        /// <summary>
        /// Lists all device types the campaign will
        /// include or exclude. NULL means target all.
        /// </summary>
        [DataMember(Name = "platform_targeting")]
        public TargetDefault PlatformTargeting { get; set; }

        /// <summary>
        /// Lists all partner account_id's exclude from 
        /// this campaign. This is exclude only. Used to 
        /// blacklist certain publishers. The account_id
        /// must reference an account with type=PARTNER.
        /// </summary>
        [DataMember(Name = "publisher_targeting")]
        public TargetDefault PublisherTargeting { get; set; }

        /// <summary>
        /// Lists all operating systems the campaign will
        /// include or exclude. This is of type ITarget
        /// to allow for two-type-format conversion in an 
        /// elegant manner.
        /// </summary>
        [DataMember(Name = "os_targeting")]
        public TargetBase OsTargeting { get; set; }

        /// <summary>
        /// Lists all connection types the campaign 
        /// will include or exclude.
        /// </summary>
        [DataMember(Name = "connection_type_targeting")]
        public TargetDefault ConnectionTypeTargeting { get; set; }

        // TODO Why exclude?
        //[DataMember(Name = "audience_segments_multi_targeting")]
        //public Target AudienceSegmentsMultiTargeting { get; set; }

        //[DataMember(Name = "custom_audience_targeting")]
        //public Target CustomAudienceTargeting { get; set; }

        //[DataMember(Name = "marking_label_multi_targeting")]
        //public Target MarkingLabelMultiTargeting { get; set; }

        /// <summary>
        /// TODO What is this?
        /// </summary>
        [DataMember(Name = "cpa_goal")]
        public decimal CpaGoal { get; set; }

        /// <summary>
        /// Campaign user comments.
        /// </summary>
        [DataMember(Name = "comments")]
        public string Note { get; set; }

        /// <summary>
        /// The amount of money spent on this campaign.
        /// </summary>
        [DataMember(Name = "spent")]
        public decimal? Spent { get; set; }

        /// <summary>
        /// The strategy we use for our bidding methods.
        /// </summary>
        [DataMember(Name = "bid_type")]
        public string BidStrategy { get; set; }

        /// <summary>
        /// The type of traffic optimization.
        /// </summary>
        [DataMember(Name = "traffic_allocation_mode")]
        public string TrafficAllocationMode { get; set; }

        /// <summary>
        /// When our campaign starts.
        /// </summary>
        [DataMember(Name = "start_date")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// When our campaign ends.
        /// </summary>
        [DataMember(Name = "end_date")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Current approval state by Taboola.
        /// </summary>
        [DataMember(Name = "approval_state")]
        public string ApprovalState { get; set; }

        /// <summary>
        /// Current campaign status.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// True if the campaign is running.
        /// </summary>
        [DataMember(Name = "is_active")]
        public bool Active { get; set; }

        /// <summary>
        /// Indicates the marketing objective of this campaign.
        /// </summary>
        [DataMember(Name = "marketing_objective")]
        public string MarketingObjective { get; set; }

    }

}
