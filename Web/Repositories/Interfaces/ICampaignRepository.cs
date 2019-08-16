using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Interfaces
{
    public interface ICampaignRepository
    {
        Task<Campaign> GetCampaign(Guid id);
        Task<List<Campaign>> GetAllCampaigns();
        Task CreateCampaignTest(Campaign c);
    }
}
