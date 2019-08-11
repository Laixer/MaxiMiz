using System;

namespace Maximiz.Model.Entity
{
    /// <summary>
    /// Advertisement item.
    /// </summary>
    [Serializable]
    public class AdItem : EntityAudit<Guid>
    {
        /// <summary>
        /// Network identifier for this object.
        /// </summary>
        public string SecondaryId { get; set; }

        /// <summary>
        /// Group to which this item belongs to.
        /// </summary>
        public int AdGroup { get; set; }

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
        /// JSON string containing unused data which
        /// we do have to store.
        /// </summary>
        public string Details { get; set; }
    }
}
