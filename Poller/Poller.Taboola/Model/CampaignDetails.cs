using Poller.Model.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Poller.Taboola.Model
{

    /// <summary>
    /// Used as a JSON mapping template. This contains
    /// all Campaign properties we do not explicitly
    /// store in our own database.
    /// </summary>
    [DataContract]
    internal class CampaignDetails
    {

        /// <summary>
        /// The account_id of the campaign's advertiser
        /// account.
        /// </summary>
        [DataMember(Name = "advertiser_id")]
        public string Account { get; set; }

        /// <summary>
        /// An object representing the wanted publisher
        /// bid modifiers for this campaign.
        /// </summary>
        [DataMember(Name = "publisher_bid_modifier")]
        public PublisherBidModifier PublisherBidModifier { get; set; }

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
        /// include or exclude.
        /// </summary>
        [DataMember(Name = "os_targeting")]
        public TargetBase OsTargeting { get; set; }

        /// <summary>
        /// Lists all connection types the campaign 
        /// will include or exclude.
        /// </summary>
        [DataMember(Name = "connection_type_targeting")]
        public TargetDefault ConnectionTypeTargeting { get; set; }

        /// <summary>
        /// Cost per action goal.
        /// TODO What is this?
        /// </summary>
        [DataMember(Name = "cpa_goal")]
        public decimal CpaGoal { get; set; }

        /// <summary>
        /// The type of traffic optimization.
        /// </summary>
        [DataMember(Name = "traffic_allocation_mode")]
        public TrafficAllocationMode TrafficAllocationMode { get; set; }

        /// <summary>
        /// True if the campaign is running.
        /// </summary>
        [DataMember(Name = "is_active")]
        public bool Active { get; set; }

        /// <summary>
        /// Indicates the marketing objective of this campaign.
        /// </summary>
        [DataMember(Name = "marketing_objective")]
        public MarketingObjective MarketingObjective { get; set; }

    }
}
