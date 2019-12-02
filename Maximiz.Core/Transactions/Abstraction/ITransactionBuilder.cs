using Maximiz.Model.Entity;
using System.Collections.Generic;

namespace Maximiz.Core.Transactions.Abstraction
{

    /// <summary>
    /// Builds transactions for us based on a list of entities we want to modify.
    /// </summary>
    public interface ITransactionBuilder
    {

        /// <summary>
        /// Builds an <see cref="EntityTransaction"/> based on a provided list
        /// of <see cref="Entity"/>s we have to process. 
        /// </summary>
        /// <param name="entities"><see cref="Entity"/> list</param>
        /// <returns><see cref="EntityTransaction"/></returns>
        EntityTransaction BuildTransaction(IEnumerable<Entity> entities);

    }
}
