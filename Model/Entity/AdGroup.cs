using System;
using Maximiz.Model.Enums;

namespace Maximiz.Model.Entity
{
    /// <summary>
    /// Group of advertisement items.
    /// </summary>
    [Serializable]
    public class AdGroup : EntityAudit<Guid>
    {
        /// <summary>
        /// Group name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User description to describe this ad group.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Array of image links.
        /// </summary>
        public string[] ImageLinks { get; set; }

        /// <summary>
        /// Array of titles.
        /// </summary>
        public string[] Titles { get; set; }

        public OperationItemStatus OperationItemStatus { get; set; }
        public string OperationItemStatusText { get => OperationItemStatus.GetEnumMemberName(); }

        public Guid OperationId { get; set; }


    }
}
