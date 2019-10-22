using Maximiz.Model.Entity;
using Maximiz.Transactions.CreateUpdateDelete;
using Maximiz.Transactions.ServiceBus;
using System;
using System.Threading.Tasks;

namespace Maximiz.Transactions
{

    /// <summary>
    /// This class handles a transaction for us, e.d. the full atomic creation
    /// of an ad group and all corresponding ad items.
    /// </summary>
    internal class TransactionHandler : ITransactionHandler
    {

        /// <summary>
        /// Manages our create, update and delete operations.
        /// </summary>
        private ICudProcessor _cudProcessor;

        /// <summary>
        /// Provides service bus connections.
        /// </summary>
        private ISender<Entity> _sender;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="cudProcessor">Object to manage entity cud operations</param>
        /// <param name="sender">Object to send items to the service bus</param>
        public TransactionHandler(ICudProcessor cudProcessor, ISender<Entity> sender)
        {
            _cudProcessor = cudProcessor;
            _sender = sender;
        }

        /// <summary>
        /// Handles an entity transaction async. This first waits for database
        /// creation, then for service bus message sent, then returns completion.
        /// </summary>
        /// <param name="transaction">The transaction to process</param>
        /// <returns>True if successful</returns>
        public async Task<bool> HandleTransaction(EntityTransaction transaction)
        {
            try
            {
                // First create all entities in the database
                foreach (var pair in transaction.EntitiesWithAccounts)
                {
                    await _cudProcessor.ProcessOperationAsync(pair.Key, transaction.CrudAction);
                }

                // Then send all entities to the service bus
                foreach (var pair in transaction.EntitiesWithAccounts)
                {
                    await _sender.SendAsync(pair.Key, pair.Value, transaction.CrudAction);
                }

                // Then mark and return true
                transaction.MarkAsComplete();
                return true;
            }

            // Return false if we failed
            catch (Exception e)
            {
                // TODO Rollback
                // TODO Log
                return false;
            }
        }

    }
}
