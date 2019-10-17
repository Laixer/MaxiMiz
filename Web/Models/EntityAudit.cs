using System;

namespace Maximiz.Model.Mvc.Entity
{
    /// <summary>
    /// Create, update, delete tracking entity.
    /// </summary>
    /// <typeparam name="TPrimary">Primary key.</typeparam>
    [Serializable]
    public abstract class EntityAudit<TPrimary> : Entity<TPrimary>
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
