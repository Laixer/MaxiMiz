using Maximiz.Database;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Abstraction
{

    /// <summary>
    /// Abstraction for a repository of campaigns.
    /// </summary>
    public interface ICampaignRepository : IEntityRepository<Campaign, Guid>
    {

        /// <summary>
        /// Gets all campaigns sorted by column in given order.
        /// </summary>
        /// <param name="column">The column to order by</param>
        /// <param name="order">Ascending or descending</param>
        /// <returns>Retrieved list of campaigns</returns>
        Task<IEnumerable<Campaign>> GetAll(ColumnCampaign column = ColumnCampaign.Name, Order order = Order.Descending);

        /// <summary>
        /// Search the database based on query string.
        /// </summary>
        /// <param name="query">The search query</param>
        /// <returns>Retrieved list of campaigns</returns>
        Task<IEnumerable<Campaign>> Search(string query);

    }
}
