using Maximiz.Core.Querying;
using Maximiz.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Repositories
{

    /// <summary>
    /// Contract for a <see cref="CampaignWithStats"/> repository.
    /// </summary>
    public interface ICampaignWithStatsRepository : IRepository<CampaignWithStats>
    {

        Task<IEnumerable<CampaignWithStats>> GetActiveAsync(QueryBase<CampaignWithStats> query);

        Task<IEnumerable<CampaignWithStats>> GetPendingAsync(QueryBase<CampaignWithStats> query);

        Task<IEnumerable<CampaignWithStats>> GetInactiveAsync(QueryBase<CampaignWithStats> query);

    }
}
