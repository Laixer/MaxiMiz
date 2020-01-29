using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Infrastructure.Database;
using Maximiz.Infrastructure.Querying;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{
    public sealed class AdGroupRepository : RepositoryBase<AdGroup>, IAdGroupRepository
    {

        public AdGroupRepository(IDatabaseProvider databaseProvider)
            : base(databaseProvider) { }

        public override Task<AdGroup> GetAsync(Guid id)
            => RepositorySharedFunctions.GetAsync<AdGroup>(_databaseProvider, id, (x) => x.Id);

        /// <summary>
        /// Gets all <see cref="AdGroup"/>s that are linked with our
        /// specified <see cref="CampaignGroup"/>.
        /// 
        /// TODO DRY with <see cref="AdGroupWithStatsRepository"/>
        /// </summary>
        /// <param name="campaignGroupId">The <see cref="CampaignGroup"/> id</param>
        /// <returns></returns>
        public Task<IEnumerable<AdGroup>> GetLinkedWithCampaignGroupAsync(Guid campaignGroupId)
        {
            if (campaignGroupId == null || Guid.Empty == campaignGroupId) { throw new ArgumentNullException(nameof(campaignGroupId)); }

            // TODO Maybe move complex queries somewhere else? Kind of hard coded, but also specific.
            var sql = $"SELECT ag.*" +
                $" FROM {QueryExtractor.GetTableName<AdGroup>()} ag" +
                $" JOIN {QueryExtractor.CampaignGroupAdGroupLinkingTableName} link" +
                $" ON ag.id = link.ad_group_id" +
                $" WHERE link.campaign_group_id = '{campaignGroupId}';";
            return RepositorySharedFunctions.QueryAsync<AdGroup>(_databaseProvider, sql);
        }

        /// <summary>
        /// Gets all <see cref="AdGroup"/>s that are linked with our
        /// specified <see cref="Campaign"/>.
        /// 
        /// TODO DRY with <see cref="AdGroupWithStatsRepository"/>
        /// </summary>
        /// <param name="campaignId">The <see cref="Campaign"/> id</param>
        /// <returns></returns>
        public Task<IEnumerable<AdGroup>> GetLinkedWithCampaignAsync(Guid campaignId)
        {
            if (campaignId == null || Guid.Empty == campaignId) { throw new ArgumentNullException(nameof(campaignId)); }

            // TODO Maybe move complex queries somewhere else? Kind of hard coded, but also specific.
            var sql = $"SELECT ag.*" +
                $" FROM {QueryExtractor.GetTableName<AdGroup>()} ag" +
                $" JOIN {QueryExtractor.CampaignAdGroupLinkingTableName} link" +
                $" ON ag.id = link.ad_group_id" +
                $" WHERE link.campaign_id = '{campaignId}';";
            return RepositorySharedFunctions.QueryAsync<AdGroup>(_databaseProvider, sql);
        }
    }
}
