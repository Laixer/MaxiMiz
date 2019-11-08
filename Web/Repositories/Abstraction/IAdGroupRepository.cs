using Maximiz.Database.Columns;
using Maximiz.Database.Querying;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Abstraction
{

    /// <summary>
    /// Interface for a <see cref="AdGroupWithStats"/> repository, which is only 
    /// responsible for performing read operations.
    /// </summary>
    public interface IAdGroupRepository : IRepository<AdGroupWithStats, QueryAdGroupWithStats, ColumnAdGroupWithStats>
    {

        /// <summary>
        /// Get all ad groups that are linked with a given campaign.
        /// </summary>
        /// <param name="campaignId">The internal id of the campaign</param>
        /// <returns>All linked ad groups</returns>
        Task<IEnumerable<AdGroupWithStats>> GetLinkedWithCampaignAsync(Guid campaignId, QueryAdGroupWithStats query);

    }
}
