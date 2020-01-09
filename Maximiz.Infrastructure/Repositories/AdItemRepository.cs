using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Infrastructure.Database;
using Maximiz.Model.Entity;
using System;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{

    /// <summary>
    /// Repository for <see cref="AdItem"/>s.
    /// </summary>
    public sealed class AdItemRepository : RepositoryBase<AdItem>, IAdItemRepository
    {

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public AdItemRepository(IDatabaseProvider databaseProvider)
            : base(databaseProvider) { }

        public override Task<AdItem> GetAsync(Guid id)
            => RepositorySharedFunctions.GetAsync<AdItem>(_databaseProvider, id, (x) => x.Id);

        public Task<AdItem> GetFromExternalIdAsync(string externalId)
            => RepositorySharedFunctions.GetFromExternalIdAsync<AdItem>(_databaseProvider, externalId, (x) => x.SecondaryId);

    }
}
