using Maximiz.Model.Entity;

namespace Maximiz.Core.Infrastructure.Repositories
{

    /// <summary>
    /// Contract for an <see cref="Account"/> repository.
    /// </summary>
    public interface IAccountWithStatsRepository : IRepository<AccountWithStats>, IRepositoryExternalIdQueryable<AccountWithStats>
    {

        //

    }
}
