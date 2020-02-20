using Maximiz.Core.Querying;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Repositories
{

    /// <summary>
    /// Contract for our <see cref="AdGroup"/> repository.
    /// </summary>
    public interface IAdGroupRepository : IRepository<AdGroup>
    {

        Task<IEnumerable<AdGroup>> GetLinkedWithCampaignGroupAsync(Guid campaignGroupId);

        Task<IEnumerable<AdGroup>> GetLinkedWithCampaignAsync(Guid campaignId);

    }
}
