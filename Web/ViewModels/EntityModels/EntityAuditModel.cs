using System;

namespace Maximiz.ViewModels.EntityModels
{

    /// <summary>
    /// Used as a base for all entity audit models used in the views.
    /// </summary>
    public abstract class EntityAuditModel<TPrimary> : EntityModel<TPrimary>
    {
        /// <summary>
        /// Timestamp of creation.
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Timestamp of last update.
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Timestamp of deletion.
        /// </summary>
        public DateTime? DeleteDate { get; set; }

    }
}
