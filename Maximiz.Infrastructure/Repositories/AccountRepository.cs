using Dapper;
using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Core.Querying;
using Maximiz.Infrastructure.Database;
using Maximiz.Infrastructure.Querying;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{

    /// <summary>
    /// Repository for <see cref="Account"/>s.
    /// </summary>
    public sealed class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public AccountRepository(IDatabaseProvider databaseProvider)
            : base(databaseProvider) { }

        /// <summary>
        /// Gets an <see cref="Account"/> based on its id.
        /// </summary>
        /// <param name="id">The account id</param>
        /// <returns><see cref="Account"/></returns>
        public override Task<Account> GetAsync(Guid id)
            => RepositorySharedFunctions.GetAsync<Account>(_databaseProvider, id, (x) => x.Id);

        /// <summary>
        /// Gets an <see cref="Account"/> based on its id used by its respective
        /// external publisher.
        /// </summary>
        /// <param name="externalId">The external id</param>
        /// <returns><see cref="Account"/></returns>
        public Task<Account> GetFromExternalIdAsync(string externalId)
            => RepositorySharedFunctions.GetFromExternalIdAsync<Account>(_databaseProvider, externalId, (x) => x.SecondaryId);

    }

}
