using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Poller.Taboola.Model
{

    /// <summary>
    /// Mirrors the Taboola AdItem result we get
    /// from the reports function 1.4 which list 
    /// all campaign items. This is coupled with 
    /// <see cref="AdItemMain"/>.
    /// </summary>
    [DataContract]
    internal class AdItemReports
    {

        /// <summary>
        /// The unique numeric item ID as a string.
        /// </summary>
        [DataMember(Name = "item")]
        public string Id { get; set; }

        /// <summary>
        /// The item name.
        /// </summary>
        [DataMember(Name = "item_name")]
        public string Title { get; set; }

        /// <summary>
        /// The url for the thumbnail of this item.
        /// </summary>
        [DataMember(Name = "thumbnail_url")]
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// The url where the item leads to.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// The numeric id of the campaign this belongs
        /// to, provided as string.
        /// </summary>
        [DataMember(Name = "campaign")]
        public string Campaign { get; set; }

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
        /// The currency in which all monetary units  are calculated, represented
        /// as a string according to the ISO 4217 standard.
        /// </summary>
        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        /// <summary>
        /// The amount of clicks on this item.
        /// </summary>
        [DataMember(Name = "clicks")]
        public long Clicks { get; set; }

        /// <summary>
        /// The amount of impressions on this item.
        /// </summary>
        [DataMember(Name = "impressions")]
        public long Impressions { get; set; }

        /// <summary>
        /// The amount spent on this item.
        /// </summary>
        [DataMember(Name = "spent")]
        public decimal Spent { get; set; }

        /// <summary>
        /// The amount of actions on the item.
        /// </summary>
        [DataMember(Name = "actions")]
        public long Actions { get; set; }

        /// <summary>
        /// Cost per click.
        /// </summary>
        [DataMember(Name = "cpc")]
        public decimal Cpc { get; set; }

    }
}
