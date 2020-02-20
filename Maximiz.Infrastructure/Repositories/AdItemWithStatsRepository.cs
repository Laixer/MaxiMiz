using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Infrastructure.Database;
using Maximiz.Model.Entity;
using System;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{
    public sealed class AdItemWithStatsRepository : RepositoryBase<AdItemWithStats>, IAdItemWithStatsRepository
    {

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public AdItemWithStatsRepository(IDatabaseProvider databaseProvider)
            : base(databaseProvider) { }

        public override Task<AdItemWithStats> GetAsync(Guid id)
            => RepositorySharedFunctions.GetAsync<AdItemWithStats>(_databaseProvider, id, (x) => x.Id);

    }
}
