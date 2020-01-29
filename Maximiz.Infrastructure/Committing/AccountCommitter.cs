using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Committing
{
    public sealed class AccountCommitter : IAccountCommitter
    {
        public Task<Account> CreateAsync(Account entity, CancellationToken token) => throw new NotImplementedException();

        public Task<Account> Delete(Account entity, CancellationToken token) => throw new NotImplementedException();

        public Task<Account> Update(Account entity, CancellationToken token) => throw new NotImplementedException();

        public Task<bool> UpdateBulkAsync(IEnumerable<Account> entities, CancellationToken token) => throw new NotImplementedException();
    }
}
