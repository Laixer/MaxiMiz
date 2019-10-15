using System.Threading.Tasks;

namespace Maximiz.Transactions
{

    /// <summary>
    /// Contract for handling an entity transaction.
    /// </summary>
    public interface ITransactionHandler
    {

        /// <summary>
        /// Handles an entity transaction async. This first waits for database
        /// creation, then for service bus message sent, then returns completion.
        /// </summary>
        /// <param name="transaction">The transaction to process</param>
        /// <returns>True if successful</returns>
        Task<bool> HandleTransaction(EntityTransaction transaction);

    }
}
