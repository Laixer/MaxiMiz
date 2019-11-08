using Maximiz.Model.Entity;
using Maximiz.Database.Querying;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Database
{

    /// <summary>
    /// Contract for database operations for our webclient.
    /// </summary>
    public interface ICrudInternalWebClient : ICrudInternal
    {

        Task<CampaignWithStats> GetCampaignWithStatsAsync(Guid guid, CancellationToken token);

        Task<AdItemWithStats> GetAdItemWithStatsAsync(Guid guid, CancellationToken token);

        Task<CampaignGroupWithStats> GetCampaignGroupAsync(Guid guid, CancellationToken token);

        Task<AdGroupWithStats> GetAdGroupAsync(Guid guid, CancellationToken token);

        Task<IEnumerable<CampaignWithStats>> GetCampaignsAsync(QueryCampaignWithStats query, int page, CancellationToken token);

        Task<IEnumerable<CampaignGroupWithStats>> GetCampaignGroupsAsync(QueryCampaignGroupWithStats query, int page, CancellationToken token);

        Task<IEnumerable<AdItemWithStats>> GetAdItemsAsync(QueryAdItemWithStats query, int page, CancellationToken token);

        Task<IEnumerable<AdGroupWithStats>> GetAdGroupsAsync(QueryAdGroupWithStats query, int page, CancellationToken token);

        Task<CampaignWithStats> CreateCampaignAsync(Campaign campaign, CancellationToken token);

        Task<CampaignGroupWithStats> CreateCampaignGroupAsync(CampaignGroup campaignGroup, CancellationToken token);

        Task<AdItemWithStats> CreateAdItemAsync(AdItem adItem, CancellationToken token);

        Task<AdGroupWithStats> CreateAdGroupAsync(AdGroup adGroup, CancellationToken token);

        Task<CampaignGroupWithStats> UpdateCampaignGroupAsync(CampaignGroup campaignGroup, CancellationToken token);

        Task<AdGroupWithStats> UpdateAdGroupAsync(AdGroup adGroup, CancellationToken token);

        Task<CampaignGroupWithStats> DeleteCampaignGroupAsync(CampaignGroup campaignGroup, CancellationToken token);

        Task<AdGroupWithStats> DeleteAdGroupAsync(AdGroup adGroup, CancellationToken token);

    }
}
