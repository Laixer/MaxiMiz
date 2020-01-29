using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Core.Querying;
using Maximiz.Infrastructure.Database;
using Maximiz.Infrastructure.Querying;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{

    /// <summary>
    /// Repository for <see cref="AdGroupWithStats"/>.
    /// </summary>
    public sealed class AdGroupWithStatsRepository : RepositoryBase<AdGroupWithStats>, IAdGroupWithStatsRepository
    {

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public AdGroupWithStatsRepository(IDatabaseProvider databaseProvider)
            : base(databaseProvider) { }

        public override Task<AdGroupWithStats> GetAsync(Guid id)
            => RepositorySharedFunctions.GetAsync<AdGroupWithStats>(_databaseProvider, id, (x) => x.Id);

        /// <summary>
        /// Gets all <see cref="AdGroupWithStats"/> that are linked with our
        /// specified <see cref="Campaign"/>.
        /// </summary>
        /// <param name="campaignId">The <see cref="Campaign"/> id</param>
        /// <returns></returns>
        public Task<IEnumerable<AdGroupWithStats>> GetLinkedWithCampaignAsync(Guid campaignId)
        {
            if (campaignId == null || Guid.Empty == campaignId) { throw new ArgumentNullException(nameof(campaignId)); }

            // TODO Maybe move complex queries somewhere else? Kind of hard coded, but also specific.
            var sql = $"SELECT a.*" +
                $" FROM {QueryExtractor.GetTableName<AdGroupWithStats>()} a" +
                $" JOIN {QueryExtractor.CampaignGroupAdGroupLinkingTableName} link" +
                $" ON a.id = link.ad_group_guid" +
                $" JOIN {QueryExtractor.GetTableName<Campaign>()} c" +
                $" ON c.campaign_group_guid = link.campaign_group_guid" +
                $" WHERE c.id = '{campaignId}';";
            return RepositorySharedFunctions.QueryAsync<AdGroupWithStats>(_databaseProvider, sql);
        }
    }
}
