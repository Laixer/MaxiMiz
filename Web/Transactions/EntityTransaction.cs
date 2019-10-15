using Maximiz.Model.Entity;
using System.Collections.Generic;

namespace Maximiz.Transactions
{

    /// <summary>
    /// Holds information over one single entity transaction.
    /// TODO How to manage account?
    /// </summary>
    public class EntityTransaction
    {

        /// <summary>
        /// List of all entities to CRUD.
        /// </summary>
        public IEnumerable<Entity> AllEntities { get; set; }

        /// <summary>
        /// True if completed successfully.
        /// </summary>
        private bool completed { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="allEntities">All entities to CRUD</param>
        public EntityTransaction(IEnumerable<Entity> allEntities)
        {
            AllEntities = allEntities;
            completed = false;
        }

        /// <summary>
        /// Marks the transaction as complete.
        /// </summary>
        public void MarkAsComplete() => completed = true;

        /// <returns>True if the transaction completed successfully</returns>
        public bool IsCompleted() => completed;

    }
}
