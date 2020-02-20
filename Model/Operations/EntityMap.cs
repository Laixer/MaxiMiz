using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

//using Entity = Maximiz.Model.Entity.Entity<System.Guid>;
// TODO This is weird. WHY

namespace Maximiz.Model.Operations
{

    /// <summary>
    /// Contains each <see cref="Entity"/> along with its <see cref="CrudAction"/> for a given <see cref="Operation"/>.
    /// </summary>
    public sealed class EntityMap
    {

        /// <summary>
        /// Contains each <see cref="Entity"/> along with its <see cref="CrudAction"/>.
        /// </summary>
        private Dictionary<Entity<Guid>, CrudAction> dictionary = new Dictionary<Entity<Guid>, CrudAction>();

        private IEnumerable<Guid> dictionaryIds => dictionary.Keys.Select(x => x.Id);

        /// <summary>
        /// Adds a new entry to this operation entity map. If the entity is 
        /// already present in the map, this will override the existing entry.
        /// </summary>
        /// <param name="entity"><see cref="Entity{TPrimary}"/></param>
        /// <param name="crudAction"><see cref="CrudAction"/></param>
        public void AddEntityWithOverride(Entity<Guid> entity, CrudAction crudAction)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            if (dictionaryIds.Contains(entity.Id))
            {
                var toRemove = Entities.Where(x => x.Id == entity.Id).First();
                dictionary.Remove(toRemove);
            }

            dictionary.Add(entity, crudAction);
        }

        /// <summary>
        /// Adds a new entry to this operation entity map.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/></param>
        /// <param name="crudAction"><see cref="CrudAction"/></param>
        public void AddEntity(Entity<Guid> entity, CrudAction crudAction)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            if (dictionaryIds.Contains(entity.Id)) { throw new InvalidOperationException(nameof(entity)); }

            dictionary.Add(entity, crudAction);
        }

        /// <summary>
        /// Gets all entities contained in this object.
        /// </summary>
        public IEnumerable<Entity<Guid>> Entities { get => dictionary.Keys; }

        /// <summary>
        /// Gets the <see cref="CrudAction"/> that belongs to a given <see cref="Entity"/>
        /// </summary>
        /// <param name="entity"><see cref="Entity"/></param>
        /// <returns><see cref="CrudAction"/></returns>
        public CrudAction GetCrudActionFromEntity(Entity<Guid> entity)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            if (!dictionaryIds.Contains(entity.Id)) { throw new InvalidOperationException($"Dictionary does not contain entity {nameof(entity)}"); }

            return dictionary[entity];
        }


    }
}
