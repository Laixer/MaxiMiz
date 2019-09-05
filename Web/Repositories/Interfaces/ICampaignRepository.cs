using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Interfaces
{
    public interface ICampaignRepository : IEntityRepository<Campaign,Guid>
    {
        Task<IEnumerable<Campaign>> GetAll();

        /// <summary>
        /// Retrieve all campaigns
        /// </summary>
        /// <returns>A list containing all the campaigns</returns>
        Task<IEnumerable<Campaign>> GetAll(string query, Order order);

        /// <summary>
        /// Get campaigns that match or contain a search query
        /// </summary>
        /// <param name="q">The search query to match</param>
        /// <returns>A list of campaigns that match the search query</returns>
        Task<IEnumerable<Campaign>> Search(string q);

        /// <summary>
        /// Create a new campaign group.
        /// </summary>
        /// <param name="entity">A campaign group entity</param>
        /// <returns></returns>
        Task CreateGroup(CampaignGroup entity);
    }
}
