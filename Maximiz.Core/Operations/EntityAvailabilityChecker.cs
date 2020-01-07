using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maximiz.Core.Operations
{

    /// <summary>
    /// Contains utility functionality to see if we can 
    /// </summary>
    public sealed class EntityAvailabilityChecker
    {

        /// <summary>
        /// Checks if a given list of <see cref="Entity"/>s are available, 
        /// meaning none of the items in the list is being processed by some
        /// other operation in our system.
        /// </summary>
        /// <param name="entities"><see cref="Entity"/></param>
        /// <returns><see cref="true"/> if successful</returns>
        internal Task<bool> AreEntitiesAvailable(IEnumerable<Entity> entities)
        {
            if (entities == null) { throw new ArgumentNullException(nameof(entities)); }

            throw new NotImplementedException();
        }

    }
}
