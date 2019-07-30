using System;

namespace Maximiz.Model
{
    /// <summary>
    /// Create, update, delete tracking entity.
    /// </summary>
    /// <typeparam name="TPrimary">Primary key.</typeparam>
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
