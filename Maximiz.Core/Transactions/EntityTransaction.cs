using Maximiz.Model;
using Maximiz.Model.Entity;
using System.Collections.Generic;

namespace Maximiz.Core.Transactions
{

    /// <summary>
    /// Holds information over one single entity transaction.
    /// </summary>
    public sealed class EntityTransaction
    {

        /// <summary>
        /// Stores key-value pairs of each entity with its corresponding account.
        /// </summary>
        public IEnumerable<KeyValuePair<Entity, Account>> EntitiesWithAccounts { get; set; }

        /// <summary>
        /// Represents the type of transaction.
        /// </summary>
        public CrudAction CrudAction { get; set; }

        /// <summary>
        /// True if completed successfully.
        /// </summary>
        private bool completed { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entitiesWithAccounts">Contains key value pairs where 
        /// each entity also has a reference to the corresponding account.</param>
        /// <param name="crudAction">The type of transaction</param>
        public EntityTransaction(IEnumerable<KeyValuePair<Entity, Account>> entitiesWithAccounts,
            CrudAction crudAction)
        {
            CrudAction = crudAction;
            EntitiesWithAccounts = entitiesWithAccounts;
            completed = false;
        }

        /// <summary>
        /// Returns a list of all entities.
        /// </summary>
        /// <returns>All present entities.</returns>
        public IEnumerable<Entity> GetAllEntities()
        {
            var result = new List<Entity>();
            foreach (var pair in EntitiesWithAccounts)
            {
                result.Add(pair.Key);
            }
            return result;
        }

        /// <summary>
        /// Marks the transaction as complete.
        /// </summary>
        public void MarkAsComplete() => completed = true;

        /// <returns>True if the transaction completed successfully</returns>
        public bool IsCompleted() => completed;

    }
}
