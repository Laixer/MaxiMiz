using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Infrastructure.Database;
using Maximiz.Model.Entity;
using System;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{
    public sealed class AdGroupRepository : RepositoryBase<AdGroup>, IAdGroupRepository
    {

        public AdGroupRepository(IDatabaseProvider databaseProvider)
            : base(databaseProvider) { }

        public override Task<AdGroup> GetAsync(Guid id)
            => RepositorySharedFunctions.GetAsync<AdGroup>(_databaseProvider, id, (x) => x.Id);

    }
}
