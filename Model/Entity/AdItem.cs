using System;
using Maximiz.Model.Enums;


namespace Maximiz.Model.Entity
{
    /// <summary>
    /// Advertisement item. This points to an actual advertisement item in one
    /// of the external API's. This belongs to one campaign, and points back
    /// to the ad group from which this was derived.
    /// </summary>
    [Serializable]
    public class AdItem : EntityAudit<Guid>
    {
        /// <summary>
        /// Network identifier for this object.
        /// </summary>
        public string SecondaryId { get; set; }

        /// <summary>
        /// Reference to the campaign this belongs to.
        /// </summary>
        public Guid CampaignGuid { get; set; }

        /// <summary>
        /// The ad group from which this item was derived.
        /// </summary>
        public int AdGroupId { get; set; }

        /// <summary>
        /// The index of the string used from the ad group.
        /// </summary>
        public int AdGroupTitleIndex { get; set; }

        /// <summary>
        /// The index of the image used from the ad group.
        /// </summary>
        public int AdGroupImageIndex { get; set; }

        /// <summary>
        /// Advertisement title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Destination URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Advertisement content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Cost per click.
        /// </summary>
        public decimal Cpc { get; set; }

        /// <summary>
        /// Spent on this item.
        /// </summary>
        public decimal Spent { get; set; }

        /// <summary>
        /// Clicks on this item. 
        /// </summary>
        public int Clicks { get; set; }

        /// <summary>
        /// Impressions on this item.
        /// </summary>
        public int Impressions { get; set; }

        /// <summary>
        /// Actions on this item.
        /// </summary>
        public int Actions { get; set; }

        /// <summary>
        /// Indicates the status of any changes made in our own database. These
        /// changes have to be pushed to the corresponding external API.
        /// </summary>
        public ApprovalState ApprovalState { get; set; }
        public string ApprovalStateText { get => ApprovalState.GetEnumMemberName(); }

        /// <summary>
        /// Represents our item status.
        /// </summary>
        public AdItemStatus Status { get; set; }
        public string StatusText { get => Status.GetEnumMemberName(); }

        /// <summary>
        /// True if this was modified beyond the properties given from the ad group.
        /// </summary>
        public bool ModifiedBeyondAdGroup { get; set; }

        /// <summary>
        /// JSON string containing unused data which
        /// we do have to store.
        /// </summary>
        public string Details { get; set; }
    }
}
