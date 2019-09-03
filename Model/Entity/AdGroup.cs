using System;
using Maximiz.Model.Enums;

namespace Maximiz.Model.Entity
{
    /// <summary>
    /// Group of advertisement items.
    /// </summary>
    [Serializable]
    public class AdGroup : EntityAudit<int>
    {
        /// <summary>
        /// Group name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Uuid string of the campaign this adgroup belongs to.
        /// </summary>
        public string CampaignUuid { get; set; }

        /// <summary>
        /// URL for all items in the group.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Description for all items in the group.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Indicates the (creation) status of this object.
        /// </summary>
        public Status Status { get; set; }
    }
}
