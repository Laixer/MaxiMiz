using Dapper;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Maximiz.Repositories
{
    public class AdItemRepository : IAdItemRepository
    {
        private readonly IConfiguration _configuration;

        public AdItemRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Returns a new instance of <see cref="NpgsqlConnection"/> for the MaxiMiz database
        /// </summary>
        public IDbConnection GetConnection => new NpgsqlConnection(_configuration.GetConnectionString("MaxiMizDatabase"));

        public Task<Guid> Create(AdItem entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(AdItem entity)
        {
            throw new NotImplementedException();
        }

        public Task<AdItem> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AdItem>> GetAds()
        {
            // TODO Determine whether this is needed
            using (IDbConnection connection = GetConnection)
            {
                var result = await connection.QueryAsync<AdItem>("SELECT * FROM ad_item ORDER BY create_date DESC LIMIT 100");
                return result;
            }
        }

        public Task<AdItem> Update(AdItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
