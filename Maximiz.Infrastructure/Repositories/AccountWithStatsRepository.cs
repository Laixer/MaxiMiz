using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Core.Querying;
using Maximiz.Infrastructure.Database;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{
    public sealed class AccountWithStatsRepository : RepositoryBase<AccountWithStats>, IAccountWithStatsRepository
    {

        public AccountWithStatsRepository(IDatabaseProvider databaseProvider)
            : base(databaseProvider) { }

        public override Task<AccountWithStats> GetAsync(Guid id)
            => RepositorySharedFunctions.GetAsync<AccountWithStats>(_databaseProvider, id, (x) => x.Id);

        public Task<AccountWithStats> GetFromExternalIdAsync(string externalId)
            => RepositorySharedFunctions.GetFromExternalIdAsync<AccountWithStats>(_databaseProvider, externalId, (x) => x.SecondaryId);

    }
}
