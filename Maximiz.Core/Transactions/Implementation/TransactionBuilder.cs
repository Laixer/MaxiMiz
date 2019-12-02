using System;
using System.Collections.Generic;
using Maximiz.Core.Transactions.Abstraction;
using Maximiz.Model.Entity;

namespace Maximiz.Core.Transactions.Implementation
{
    internal sealed class TransactionBuilder : ITransactionBuilder
    {

        private readonly ITransactionExecuter _transactionExecuter;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="transactionExecuter"><see cref="ITransactionExecuter"/></param>
        public TransactionBuilder(ITransactionExecuter transactionExecuter)
        {
            _transactionExecuter = transactionExecuter;
        }

        public EntityTransaction BuildTransaction(IEnumerable<Entity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
