using System.Runtime.Serialization;

namespace Poller.Taboola.Model
{

    /// <summary>
    /// Used as a template for the JSON conversion of parameters that we do not 
    /// store explicitly.
    /// </summary>
    [DataContract]
    internal class AdItemDetails
    {

        /// <summary>
        /// Unique numeric ID of the items parent campaign returned as a string. 
        /// This is ported from both <see cref="AdItemExternal.CampaignId"/> and from
        /// <see cref="AdItemReports.Campaign"/>. These two parameters are the
        /// same, but get extracted differently in the two different API calls.
        /// </summary>
        [DataMember(Name = "campaign_id")]
        public string CampaignId { get; set; }

        /// <summary>
        /// Whether the item is still active.
        /// </summary>
        [DataMember(Name = "is_active")]
        public bool Active { get; set; }

        /// <summary>
        /// Represents the type of ad item. We do not use this at the moment.
        /// This should be ported to <see cref="AdItemType"/> when we need it.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// The name the campaign this item belongs to in human readable form.
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
        /// The currency in which all monetary units  are calculated, represented
        /// as a string according to the ISO 4217 standard.
        /// </summary>
        [DataMember(Name = "currency")]
        public string Currency { get; set; }

    }
}
