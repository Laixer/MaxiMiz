using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Repositories
{

    /// <summary>
    /// Contains mock functions to return lists.
    /// </summary>
    public class MockCampaignRepository : ICampaignRepository
    {

        public async Task<CampaignWithStats> Get(Guid id) => GetCampaignOne();
         
        public async Task<IEnumerable<CampaignWithStats>> GetActive(int page)
        {
            var result = new List<CampaignWithStats>();
            result.Add(GetCampaignOne());
            result.Add(GetCampaignOne());
            return result;
        }

        public async Task<IEnumerable<CampaignWithStats>> GetAll(int page)
        {
            var result = new List<CampaignWithStats>();
            result.Add(GetCampaignOne());
            result.Add(GetCampaignTwo());
            result.Add(GetCampaignOne());
            result.Add(GetCampaignTwo());
            result.Add(GetCampaignOne());
            result.Add(GetCampaignTwo());
            return result;
        }

        public async Task<IEnumerable<CampaignWithStats>> GetConcept(int page)
        {
            var result = new List<CampaignWithStats>();
            result.Add(GetCampaignTwo());
            return result;
        }

        public async Task<IEnumerable<CampaignWithStats>> GetDeleted(int page)
        {
            var result = new List<CampaignWithStats>();
            result.Add(GetCampaignTwo());
            return result;
        }

        public async Task<IEnumerable<CampaignWithStats>> GetHidden(int page)
        {
            var result = new List<CampaignWithStats>();
            result.Add(GetCampaignOne());
            result.Add(GetCampaignOne());
            return result;
        }

        public async Task<IEnumerable<CampaignWithStats>> GetInactive(int page)
        {
            var result = new List<CampaignWithStats>();
            result.Add(GetCampaignTwo());
            return result;
        }

        public async Task<IEnumerable<CampaignWithStats>> GetPending(int page)
        {
            var result = new List<CampaignWithStats>();
            result.Add(GetCampaignTwo());
            result.Add(GetCampaignTwo());
            result.Add(GetCampaignTwo());
            return result;
        }

        public Task QueryOnLoad()
        {
            throw new NotImplementedException();
        }

        private CampaignWithStats GetCampaignOne() => new CampaignWithStats
        {
            Id = Guid.NewGuid(),
            Name = "Dummy campaign one",
            Budget = 54,
            Spent = 1000,
            Clicks = 18548,
            Roi = 66.49,
            Revenue = 1337,
            RevenueAdsense = 1,
            RevenueTaboola = 1336,
            Profit = 337,
            Actions = 1574,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now
        };
        private CampaignWithStats GetCampaignTwo() => new CampaignWithStats
        {
            Id = Guid.NewGuid(),
            Name = "Dummy campaign two",
            Budget = 185,
            Spent = 8000,
            Clicks = 183748,
            Roi = 32.45,
            Revenue = 16657,
            RevenueAdsense = 10000,
            RevenueTaboola = 6657,
            Profit = 8657,
            Actions = 1574,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now
        };
    }

}
