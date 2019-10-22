using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Transactions
{

    /// <summary>
    /// Builds transactions for us based on a list of entities we want to modify.
    /// </summary>
    internal interface ITransactionBuilder
    {

        Task<bool> BuildAndRunTransactionAsync(IEnumerable<Entity> entities);

    }
}
