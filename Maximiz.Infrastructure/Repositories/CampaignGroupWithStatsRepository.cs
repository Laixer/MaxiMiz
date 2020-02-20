using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Infrastructure.Database;
using Maximiz.Model.Entity;
using System;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{
    public sealed class CampaignGroupWithStatsRepository : RepositoryBase<CampaignGroupWithStats>, ICampaignGroupWithStatsRepository
    {

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignGroupWithStatsRepository(IDatabaseProvider databaseProvider)
            : base(databaseProvider) { }

        public override Task<CampaignGroupWithStats> GetAsync(Guid id)
            => RepositorySharedFunctions.GetAsync<CampaignGroupWithStats>(_databaseProvider, id, (x) => x.Id);

    }
}
