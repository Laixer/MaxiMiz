using Dapper;
using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Infrastructure.Database;
using Maximiz.Infrastructure.Querying;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{

    /// <summary>
    /// Repository for getting <see cref="Campaign"/>s.
    /// Inherits from <see cref="ICampaignRepository"/>.
    /// </summary>
    public sealed class CampaignRepository : RepositoryBase<Campaign>, ICampaignRepository
    {

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignRepository(IDatabaseProvider databaseProvider)
            : base(databaseProvider) { }

        public override Task<Campaign> GetAsync(Guid id)
            => RepositorySharedFunctions.GetAsync<Campaign>(_databaseProvider, id, (x) => x.Id);

        public Task<Campaign> GetFromExternalIdAsync(string externalId)
            => RepositorySharedFunctions.GetFromExternalIdAsync<Campaign>(_databaseProvider, externalId, (x) => x.SecondaryId);

        /// <summary>
        /// Gets all <see cref="Campaign"/>s linked with a given <see cref="CampaignGroup"/>./
        /// </summary>
        /// <param name="campaignGroupId"><see cref="CampaignGroup"/> database id</param>
        /// <returns><see cref="IEnumerable{Campaign}"/></returns>
        public async Task<IEnumerable<Campaign>> GetLinkedWithCampaignGroupAsync(Guid campaignGroupId)
        {
            if (campaignGroupId == null || campaignGroupId == Guid.Empty) { throw new ArgumentNullException(nameof(campaignGroupId)); }

            using (var connection = _databaseProvider.GetConnectionScope())
            {
                var sql = $"SELECT c.*" +
                    $" FROM {QueryExtractor.GetTableName<Campaign>()} c" +
                    $" JOIN {QueryExtractor.GetTableName<CampaignGroup>()} cg" +
                    $" ON c.campaign_group_guid = cg.id" +
                    $" WHERE cg.id = '{campaignGroupId}';"; // TODO harcode?
                return await connection.QueryAsync<Campaign>(new CommandDefinition(sql));
            }
        }

    }

}
