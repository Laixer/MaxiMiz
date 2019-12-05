using Dapper;
using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Core.Querying;
using Maximiz.Infrastructure.Database;
using Maximiz.Infrastructure.Repositories.Sql;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{

    /// <summary>
    /// Repository for getting <see cref="Campaign"/>s.
    /// Inherits from <see cref="ICampaignRepository"/>.
    /// </summary>
    public sealed class CampaignRepository : ICampaignRepository
    {

        private readonly IDatabaseProvider _databaseProvider;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignRepository(IDatabaseProvider databaseProvider)
        {
            if (databaseProvider == null) { throw new ArgumentNullException(nameof(databaseProvider)); }
            _databaseProvider = databaseProvider;
        }

        /// <summary>
        /// Gets a <see cref="Campaign"/> based on its internal id.
        /// </summary>
        /// <param name="id">Internal id</param>
        /// <returns><see cref="Campaign"/></returns>
        public async Task<Campaign> GetAsync(Guid id)
        {
            if (id == null) { throw new ArgumentNullException(nameof(id)); }

            using (var connection = _databaseProvider.GetConnectionScope())
            {
                var result = (await connection.QueryAsync<Campaign>(
                    SqlCampaignRepository.GetCampaign, id)).FirstOrDefault();
                if (result == null) { throw new InvalidOperationException($"Could not find campaign with id {id}"); }
                return result;
            }
        }

        /// <summary>
        /// Gets a <see cref="Campaign"/> based on its external id.
        /// </summary>
        /// <param name="externalId">External id</param>
        /// <returns><see cref="Campaign"/></returns>
        public async Task<Campaign> GetFromExternalIdAsync(string externalId)
        {
            if (externalId == null) { throw new ArgumentNullException(nameof(externalId)); }

            using (var connection = _databaseProvider.GetConnectionScope())
            {
                var result = (await connection.QueryAsync<Campaign>(
                    SqlCampaignRepository.GetCampaignFromExternalId, externalId)).FirstOrDefault();
                if (result == null) { throw new InvalidOperationException($"Could not find campaign with external id {externalId}"); }
                return result;
            }
        }

        /// <summary>
        /// Gets all <see cref="Campaign"/>s from our database.
        /// TODO Is this smart? Big table might become a problem?
        /// </summary>
        /// <returns><see cref="IEnumerable{Campaign}"/></returns>
        public async Task<IEnumerable<Campaign>> GetAllAsync()
        {
            using (var connection = _databaseProvider.GetConnectionScope())
            {
                return await connection.QueryAsync<Campaign>(new CommandDefinition(
                    SqlCampaignRepository.GetAllCampaigns));
            }
        }

        /// <summary>
        /// Returns a list of <see cref="Campaign"/>s based on some query.
        /// </summary>
        /// <remarks>
        /// If the query is set to null, this function returns <see cref="GetAllAsync"/>.
        /// </remarks>
        /// <param name="query"><see cref="QueryBase{Campaign}"/></param>
        /// <returns><see cref="IEnumerable{Campaign}"/></returns>
        public Task<IEnumerable<Campaign>> GetAllAsync(QueryBase<Campaign> query)
        {
            if (query == null) { return GetAllAsync(); }

            throw new NotImplementedException();
        }

    }
}
