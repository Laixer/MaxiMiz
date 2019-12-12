using Maximiz.Model;
using System;
using System.Collections.Generic;

using Entity = Maximiz.Model.Entity.Entity<System.Guid>;
// TODO This is weird. WHY

namespace Maximiz.Core.Operations
{

    /// <summary>
    /// Contains each <see cref="Entity"/> along with its <see cref="CrudAction"/> for a given <see cref="Operation"/>.
    /// </summary>
    public sealed class EntityMap
    {

        /// <summary>
        /// Contains each <see cref="Entity"/> along with its <see cref="CrudAction"/>.
        /// </summary>
        private Dictionary<Entity, CrudAction> dictionary = new Dictionary<Entity, CrudAction>();

        /// <summary>
        /// Adds a new entry to this operation entity map.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/></param>
        /// <param name="crudAction"><see cref="CrudAction"/></param>
        public void AddEntity(Entity entity, CrudAction crudAction)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            if (dictionary.ContainsKey(entity)) { return; } // TODO Or throw?

            dictionary.Add(entity, crudAction);
        }

        /// <summary>
        /// Gets all entities contained in this object.
        /// </summary>
        public IEnumerable<Entity> Entities { get => dictionary.Keys; }

        /// <summary>
        /// Gets the <see cref="CrudAction"/> that belongs to a given <see cref="Entity"/>
        /// </summary>
        /// <param name="entity"><see cref="Entity"/></param>
        /// <returns><see cref="CrudAction"/></returns>
        public CrudAction GetCrudActionFromEntity(Entity entity)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            if (!dictionary.ContainsKey(entity)) { throw new InvalidOperationException($"Dictionary does not contain entity {nameof(entity)}"); }

            return dictionary[entity];
        }


    }
}
