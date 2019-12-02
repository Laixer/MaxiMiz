using Maximiz.Model.Entity;

namespace Maximiz.Core.Infrastructure.Repositories
{

    /// <summary>
    /// Contract for a <see cref="Campaign"/> repository.
    /// </summary>
    public interface ICampaignRepository : IRepository<Campaign>, IRepositoryExternalIdQueryable<Campaign>
    {

        //

    }
}
