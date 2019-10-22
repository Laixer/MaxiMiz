
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Maximiz.Model.Entity;

namespace Maximiz.Database
{

    /// <summary>
    /// TODO Implement.
    /// </summary>
    public class CrudInternal : ICrudInternal
    {
        public Task<AdItem> DeleteAdItemAsync(AdItem adItem, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Campaign> DeleteCampaignAsync(Campaign campaign, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<AdItem> GetAdItemAsync(Guid guid, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<AdItem> GetAdItemAsync(string externalId, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Campaign> GetCampaignAsync(Guid guid, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Campaign> GetCampaignAsync(string externalId, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<AdItem> UpdateAdItemAsync(AdItem adItem, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Campaign> UpdateCampaignAsync(Campaign campaign, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
