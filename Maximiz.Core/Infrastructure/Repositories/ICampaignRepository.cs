using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Repositories
{

    /// <summary>
    /// Contract for a <see cref="Campaign"/> repository.
    /// </summary>
    public interface ICampaignRepository : IRepository<Campaign>, IRepositoryExternalIdQueryable<Campaign>
    {

        Task<IEnumerable<Campaign>> GetLinkedWithCampaignGroupAsync(Guid campaignGroupId);

    }
}
