using Maximiz.Database.Columns;
using Maximiz.Database.Querying;
using Maximiz.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Abstraction
{

    /// <summary>
    /// Interface for a <see cref="CampaignWithStats"/> repository, which is only responsible
    /// for performing read operations.
    /// TODO Doc.
    /// </summary>
    public interface ICampaignRepository : IRepository<CampaignWithStats, QueryCampaignWithStats, ColumnCampaignWithStats>
    {

        /// <summary>
        /// Query the data store to get all active campaigns.
        /// </summary>
        /// <param name="query"><see cref="QueryCampaignWithStats"/></param>
        /// <param name="page">The page number</param>
        /// <returns>List of queried <see cref="CampaignWithStats"/></returns>
        Task<IEnumerable<CampaignWithStats>> GetActiveAsync(QueryCampaignWithStats query, int page = 0);

        /// <summary>
        /// Query the data store to get all inactive campaigns.
        /// </summary>
        /// <param name="query"><see cref="QueryCampaignWithStats"/></param>
        /// <param name="page">The page number</param>
        /// <returns>List of queried <see cref="CampaignWithStats"/></returns>
        Task<IEnumerable<CampaignWithStats>> GetInactiveAsync(QueryCampaignWithStats query, int page = 0);

        /// <summary>
        /// Query the data store to get all pending campaigns.
        /// </summary>
        /// <param name="query"><see cref="QueryCampaignWithStats"/></param>
        /// <param name="page">The page number</param>
        /// <returns>List of queried <see cref="CampaignWithStats"/></returns>
        Task<IEnumerable<CampaignWithStats>> GetPendingAsync(QueryCampaignWithStats query, int page = 0);

    }

}
