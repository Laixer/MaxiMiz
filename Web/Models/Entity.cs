using System;

namespace Maximiz.MVc.Model.Entity
{
    [Serializable]
    public abstract class Entity { }

    /// <summary>
    /// Datastore entity.
    /// </summary>
    /// <typeparam name="TPrimary">Primary key.</typeparam>
    [Serializable]
    public abstract class Entity<TPrimary> : Entity
    {
        /// <summary>
        /// Entity identifier.
        /// </summary>
        public TPrimary Id { get; set; }
    }
}
