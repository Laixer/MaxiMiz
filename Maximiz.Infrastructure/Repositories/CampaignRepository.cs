using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Infrastructure.Database;
using Maximiz.Model.Entity;
using System;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{

    /// <summary>
    /// Repository for getting <see cref="Campaign"/>s.
    /// Inherits from <see cref="ICampaignRepository"/>.
    /// </summary>
    public sealed class CampaignRepository : RepositoryBase<Campaign>, ICampaignRepository
    {

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignRepository(IDatabaseProvider databaseProvider)
            : base(databaseProvider) { }

        public override Task<Campaign> GetAsync(Guid id)
            => RepositorySharedFunctions.GetAsync<Campaign>(_databaseProvider, id, (x) => x.Id);

        public Task<Campaign> GetFromExternalIdAsync(string externalId)
            => RepositorySharedFunctions.GetFromExternalIdAsync<Campaign>(_databaseProvider, externalId, (x) => x.SecondaryId);

    }

}
