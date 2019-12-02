using System.Threading.Tasks;

namespace Maximiz.Core.Transactions.Abstraction
{

    /// <summary>
    /// Contract for executing an entity transaction.
    /// </summary>
    public interface ITransactionExecuter
    {

        /// <summary>
        /// Executes an entity transaction async. This first waits for database
        /// creation, then for service bus message sent, then returns completion.
        /// </summary>
        /// <param name="transaction"><see cref="EntityTransaction"/></param>
        /// <returns>True if successful</returns>
        Task<bool> ExecuteTransactionAsync(EntityTransaction transaction);

    }
}
