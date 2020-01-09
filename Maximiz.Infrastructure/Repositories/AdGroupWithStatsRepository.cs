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
        /// <param name="campaignId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public Task<IEnumerable<AdGroupWithStats>> GetLinkedWithCampaignAsync(Guid campaignId, QueryBase<AdGroupWithStats> query)
        {
            if (campaignId == null || Guid.Empty == campaignId) { throw new ArgumentNullException(nameof(campaignId)); }

            // TODO This should never happen!!!! REMOVE THIS DEBUG
            campaignId = new Guid("ca456d7e-ec2a-425b-b01e-dda04939ef7e");

            // TODO Maybe move complex queries somewhere else? Kind of hard coded, but also specific.
            var sql = $"SELECT a.*" +
                $" FROM {QueryExtractor.GetTableName<AdGroupWithStats>()} a" +
                $" JOIN {QueryExtractor.CampaignGroupAdGroupLinkingTableName} link" +
                $" ON a.id = link.ad_group_guid" +
                $" JOIN {QueryExtractor.GetTableName<Campaign>()} c" +
                $" ON c.campaign_group_guid = link.campaign_group_id" +
                $" WHERE c.id = '{campaignId}';";
            return RepositorySharedFunctions.QueryAsync<AdGroupWithStats>(_databaseProvider, sql);
        }
    }
}
