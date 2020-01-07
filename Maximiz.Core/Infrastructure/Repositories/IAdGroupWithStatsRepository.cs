using Maximiz.Core.Querying;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Repositories
{

    /// <summary>
    /// Contract for an <see cref="AdGroupWithStats"/> repository.
    /// </summary>
    public interface IAdGroupWithStatsRepository : IRepository<AdGroupWithStats>
    {

        Task<IEnumerable<AdGroupWithStats>> GetLinkedWithCampaignAsync(Guid campaignId, QueryBase<AdGroupWithStats> query);

    }
}
