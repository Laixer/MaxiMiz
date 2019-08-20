using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Interfaces
{
    public interface ICampaignRepository : IEntityRepository<Campaign,Guid>
    {
        /// <summary>
        /// Retrieve all campaigns
        /// </summary>
        /// <returns>A list containing all the campaigns</returns>
        IEnumerable<Campaign> GetAll();

        /// <summary>
        /// Get campaigns that match or contain a search query
        /// </summary>
        /// <param name="q">The search query to match</param>
        /// <returns>A list of campaigns that match the search query</returns>
        IEnumerable<Campaign> Search(string q);
    }
}
