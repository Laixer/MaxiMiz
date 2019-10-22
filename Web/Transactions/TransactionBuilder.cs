using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maximiz.AccountManagement;
using Maximiz.Model.Entity;

namespace Maximiz.Transactions
{
    internal class TransactionBuilder : ITransactionBuilder
    {

        private IAccountManager accountManager;
        private ITransactionHandler transactionHandler;

        public TransactionBuilder(IAccountManager manager, ITransactionHandler handler)
        {
            accountManager = manager;
            transactionHandler = handler;
        }

        public Task<bool> BuildAndRunTransactionAsync(IEnumerable<Entity> entities)
        {
            throw new NotImplementedException();
        }

    }
}
