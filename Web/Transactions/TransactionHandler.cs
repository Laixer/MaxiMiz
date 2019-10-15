using Maximiz.Model.Entity;
using Maximiz.Transactions.Creation;
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
        /// Provides database connections.
        /// </summary>
        private ICreator<Entity> _creator;

        /// <summary>
        /// Provides service bus connections.
        /// </summary>
        private ISender<Entity> _sender;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="creator">Object to create items in our database</param>
        /// <param name="sender">Object to send items to the service bus</param>
        public TransactionHandler(ICreator<Entity> creator, ISender<Entity> sender)
        {
            _creator = creator;
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
                foreach (var entity in transaction.AllEntities)
                {
                    await _creator.Create(entity);
                }

                // Then send all entities to the service bus
                foreach (var entity in transaction.AllEntities)
                {
                    await _sender.Send(entity);
                }

                // Then return true
                transaction.MarkAsComplete();
                return true;
            }

            // Return false if we failed
            catch (Exception e)
            {
                // TODO Log
                return false;
            }
        }

    }
}
