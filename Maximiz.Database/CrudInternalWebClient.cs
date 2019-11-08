using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Maximiz.Database.Querying;
using Maximiz.Model.Entity;

namespace Maximiz.Database
{

    /// <summary>
    /// TODO Implement.
    /// </summary>
    public class CrudInternalWebClient : CrudInternal, ICrudInternalWebClient
    {
        public Task<AdGroupWithStats> CreateAdGroupAsync(AdGroup adGroup, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<AdItemWithStats> CreateAdItemAsync(AdItem adItem, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<CampaignWithStats> CreateCampaignAsync(Campaign campaign, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<CampaignGroupWithStats> CreateCampaignGroupAsync(CampaignGroup campaignGroup, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<AdGroupWithStats> DeleteAdGroupAsync(AdGroup adGroup, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<CampaignGroupWithStats> DeleteCampaignGroupAsync(CampaignGroup campaignGroup, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<AdGroupWithStats> GetAdGroupAsync(Guid guid, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AdGroupWithStats>> GetAdGroupsAsync(QueryAdGroupWithStats query, int page, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AdItemWithStats>> GetAdItemsAsync(QueryAdItemWithStats query, int page, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<AdItemWithStats> GetAdItemWithStatsAsync(Guid guid, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<CampaignGroupWithStats> GetCampaignGroupAsync(Guid guid, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CampaignGroupWithStats>> GetCampaignGroupsAsync(QueryCampaignGroupWithStats query, int page, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CampaignWithStats>> GetCampaignsAsync(QueryCampaignWithStats query, int page, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<CampaignWithStats> GetCampaignWithStatsAsync(Guid guid, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<AdGroupWithStats> UpdateAdGroupAsync(AdGroup adGroup, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<CampaignGroupWithStats> UpdateCampaignGroupAsync(CampaignGroup campaignGroup, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
