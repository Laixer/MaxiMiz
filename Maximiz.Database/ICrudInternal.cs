using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Database
{

    /// <summary>
    /// Contract for operating on the internal database.
    /// TODO Doc
    /// </summary>
    public interface ICrudInternal
    {

        Task<Campaign> GetCampaignAsync(Guid guid, CancellationToken token);

        Task<Campaign> GetCampaignAsync(string externalId, CancellationToken token);

        Task<AdItem> GetAdItemAsync(Guid guid, CancellationToken token);

        Task<AdItem> GetAdItemAsync(string externalId, CancellationToken token);

        Task<Campaign> UpdateCampaignAsync(Campaign campaign, CancellationToken token);

        Task<Campaign> DeleteCampaignAsync(Campaign campaign, CancellationToken token);

        Task<AdItem> UpdateAdItemAsync(AdItem adItem, CancellationToken token);

        Task<AdItem> DeleteAdItemAsync(AdItem adItem, CancellationToken token);

    }
}
