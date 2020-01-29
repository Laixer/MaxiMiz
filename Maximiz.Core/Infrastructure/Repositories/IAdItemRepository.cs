using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Repositories
{

    /// <summary>
    /// Contract for an <see cref="AdItem"/> repository.
    /// </summary>
    public interface IAdItemRepository : IRepository<AdItem>, IRepositoryExternalIdQueryable<AdItem>
    {

        Task<IEnumerable<AdItem>> GetAllFromAdGroupAndCampaignAsync(Guid adGroupId, Guid campaignId);

        Task<IEnumerable<AdItem>> GetAllFromCampaignAsync(Guid campaignId);

    }
}
