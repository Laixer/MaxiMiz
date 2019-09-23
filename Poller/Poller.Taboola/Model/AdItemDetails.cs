using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Poller.Taboola.Model
{

    /// <summary>
    /// Used as a template for the JSON conversion 
    /// of parameters that we do not store explicitly.
    /// </summary>
    [DataContract]
    internal class AdItemDetails
    {

        /// <summary>
        /// Unique numeric ID of the items parent campaign
        /// returned as a string. This is ported from both
        /// <see cref="AdItemMain.CampaignId"/> and from
        /// <see cref="AdItemReports.Campaign"/>. These 
        /// two parameters are the same, but get extracted
        /// differently in different API calls.
        /// </summary>
        [DataMember(Name = "campaign_id")]
        public string CampaignId { get; set; }

        /// <summary>
        /// Whether the item is still active.
        /// </summary>
        [DataMember(Name = "is_active")]
        public bool Active { get; set; }

        /// <summary>
        /// Item approval status.
        /// </summary>
        [DataMember(Name = "approval_state")]
        public ApprovalState ApprovalState { get; set; }

        /// <summary>
        /// Ad item status.
        /// </summary>
        [DataMember(Name = "status")]
        public CampaignItemStatus CampaignItemStatus { get; set; }

        /// <summary>
        /// The url for the thumbnail of this item.
        /// </summary>
        [DataMember(Name = "thumbnail_url")]
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// The name the campaign this item belongs to
        /// in human readable form.
        /// </summary>
        [DataMember(Name = "campaign_name")]
        public string CampaignName { get; set; }

        /// <summary>
        /// Machine-readable advertiser name.
        /// </summary>
        [DataMember(Name = "content_provider")]
        public string ContentProvider { get; set; }

        /// <summary>
        /// Human-readable advertiser name.
        /// </summary>
        [DataMember(Name = "content_provider_name")]
        public string ContentProviderName { get; set; }

        /// <summary>
        /// The amount of clicks on this item.
        /// </summary>
        [DataMember(Name = "clicks")]
        public long Clicks { get; set; }

        /// <summary>
        /// Cost per 1000 impressions.
        /// </summary>
        [DataMember(Name = "cpm")]
        public decimal Cpm { get; set; }

        /// <summary>
        /// The currency in which all monetary units 
        /// are calculated, represented as a string.
        /// </summary>
        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Cost per action.
        /// </summary>
        [DataMember(Name = "cpa")]
        public decimal Cpa { get; set; }

        /// <summary>
        /// Conversion rate.
        /// </summary>
        [DataMember(Name = "cvr")]
        public decimal Cvr { get; set; }

    }
}
