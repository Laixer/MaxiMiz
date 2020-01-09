using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Infrastructure.Database;
using Maximiz.Model.Entity;
using System;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{
    public sealed class CampaignGroupRepository : RepositoryBase<CampaignGroup>, ICampaignGroupRepository
    {

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignGroupRepository(IDatabaseProvider databaseProvider)
            : base(databaseProvider) { }

        public override Task<CampaignGroup> GetAsync(Guid id)
            => RepositorySharedFunctions.GetAsync<CampaignGroup>(_databaseProvider, id, (x) => x.Id);

    }
}
