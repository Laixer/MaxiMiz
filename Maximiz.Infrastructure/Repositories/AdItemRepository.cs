using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Infrastructure.Database;
using Maximiz.Infrastructure.Querying;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{

    /// <summary>
    /// Repository for <see cref="AdItem"/>s.
    /// </summary>
    public sealed class AdItemRepository : RepositoryBase<AdItem>, IAdItemRepository
    {

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public AdItemRepository(IDatabaseProvider databaseProvider)
            : base(databaseProvider) { }

        public override Task<AdItem> GetAsync(Guid id)
            => RepositorySharedFunctions.GetAsync<AdItem>(_databaseProvider, id, (x) => x.Id);

        public Task<AdItem> GetFromExternalIdAsync(string externalId)
            => RepositorySharedFunctions.GetFromExternalIdAsync<AdItem>(_databaseProvider, externalId, (x) => x.SecondaryId);

        /// <summary>
        /// Gets all ad items that belong to a given <paramref name="adGroupId"/> 
        /// and also to a given <paramref name="campaignId"/>.
        /// </summary>
        /// <param name="adGroupId"><see cref="AdGroup"/> id</param>
        /// <param name="campaignId"><see cref="Campaign"/> id</param>
        /// <returns><see cref="IEnumerable{AdItem}"/></returns>
        public Task<IEnumerable<AdItem>> GetAllFromAdGroupAndCampaignAsync(Guid adGroupId, Guid campaignId)
        {
            if (campaignId == null || campaignId == Guid.Empty) { throw new ArgumentNullException(nameof(campaignId)); }
            if (adGroupId == null || adGroupId == Guid.Empty) { throw new ArgumentNullException(nameof(adGroupId)); }

            var sql = $"SELECT * FROM {QueryExtractor.GetTableName<AdItem>()} " +
                $"WHERE campaign_guid = '{campaignId}' AND ad_group_guid = '{adGroupId}';"; 
            return RepositorySharedFunctions.QueryAsync<AdItem>(_databaseProvider, sql);
        }

        /// <summary>
        /// Gets all <see cref="AdItem"/>s that are linked to a given <see cref="Campaign"/>.
        /// </summary>
        /// <param name="campaignId"><see cref="Campaign"/> id</param>
        /// <returns><see cref="IEnumerable{AdItem}"/></returns>
        public Task<IEnumerable<AdItem>> GetAllFromCampaignAsync(Guid campaignId)
        {
            if (campaignId == null || campaignId == Guid.Empty) { throw new ArgumentNullException(nameof(campaignId)); }

            // TODO Maybe more centralized, this is hard coded
            var sql = $"SELECT * FROM {QueryExtractor.GetTableName<AdItem>()} WHERE campaign_guid = '{campaignId}';"; 
            return RepositorySharedFunctions.QueryAsync<AdItem>(_databaseProvider, sql);
        }
    }
}
