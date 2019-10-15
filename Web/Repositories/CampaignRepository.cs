using Dapper;
using Laixer.Library.Injection.Database;
using Maximiz.Database;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Maximiz.Repositories
{

    /// <summary>
    /// Repository layer for operations related to <see cref="Campaign"/> data.
    /// TODO Fix ugly interface implementation
    /// </summary>
    public class CampaignRepository : ICampaignRepository
    {

        /// <summary>
        /// Provides database connections for us.
        /// </summary>
        private readonly IDatabaseProvider _databaseProvider;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="databaseProvider">The database provider</param>
        public CampaignRepository(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        /// <summary>
        /// Gets all campaigns sorted by a column in given order.
        /// </summary>
        /// <param name="column">The column to order by</param>
        /// <param name="order">Ascending or descending</param>
        /// <returns>Retrieved list of campaigns</returns>
        async Task<IEnumerable<Campaign>> ICampaignRepository.GetAll(ColumnCampaign column, Order order)
        {
            using (IDbConnection connection = _databaseProvider.GetConnectionScope())
            {
                var columnString = new OrderTranslator().Convert(column);
                var orderString = new OrderTranslator().Convert(order);
                try
                {
                    var sql = $"SELECT * FROM public.campaign ORDER BY {columnString} {orderString} LIMIT 100";
                    var result = await connection.QueryAsync<Campaign>(sql);
                    return result;
                }
                catch (Exception e) { throw; }
            }
        }

        /// <summary>
        /// Search the database based on query string. At the moment this only
        /// targets the name and the branding text.
        /// 
        /// TODO Implement other kind of searching as well.
        /// TODO Consider performance with UPPER strings
        /// </summary>
        /// <param name="query">The search query</param>
        /// <returns>Retrieved list of campaigns</returns>
        async Task<IEnumerable<Campaign>> ICampaignRepository.Search(string query)
        {
            try
            {
                using (var connection = _databaseProvider.GetConnectionScope())
                {
                    var sql = $"SELECT * FROM campaign " +
                        $"WHERE UPPER(name) LIKE UPPER('%{query}%') " +
                        $"OR UPPER(branding_text) LIKE UPPER('%{query}%') " +
                        $"LIMIT 100";
                    var result = await connection.QueryAsync<Campaign>(sql);
                    return result;
                }
            }
            catch (Exception e) { throw; }
        }

        /// <summary>
        /// Get a specific campaign out the database.
        /// </summary>
        /// <param name="id">the id of the campaign</param>
        /// <returns>one specific campaign in the database</returns>
        public async Task<Campaign> Get(Guid id)
        {
            using (IDbConnection connection = _databaseProvider.GetConnectionScope())
            {
                var result = await connection.QueryFirstOrDefaultAsync<Campaign>(@"
                    SELECT * FROM campaign WHERE id = @Id", new { Id = id });
                return result;
            }
        }

        public Task<Guid> Create(Campaign entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Campaign entity)
        {
            throw new NotImplementedException();
        }

        public Task<Campaign> Update(Campaign entity)
        {
            throw new NotImplementedException();
        }

    }
}
