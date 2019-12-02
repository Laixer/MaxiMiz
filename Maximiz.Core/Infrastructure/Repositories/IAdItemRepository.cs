using Maximiz.Model.Entity;

namespace Maximiz.Core.Infrastructure.Repositories
{

    /// <summary>
    /// Contract for an <see cref="AdItem"/> repository.
    /// </summary>
    public interface IAdItemRepository : IRepository<AdItem>, IRepositoryExternalIdQueryable<AdItem>
    {

        //

    }
}
