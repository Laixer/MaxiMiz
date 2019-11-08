using Maximiz.Database.Querying;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Mock
{

    /// <summary>
    /// Contains mock functions to return lists.
    /// </summary>
    public class MockCampaignRepository : ICampaignRepository
    {

        public async Task<CampaignWithStats> Get(Guid id) => GetCampaignOne();

        public async Task<IEnumerable<CampaignWithStats>> GetAllAsync(QueryCampaignWithStats query, int page = 0)
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

        public async Task<IEnumerable<CampaignWithStats>> GetActiveAsync(QueryCampaignWithStats query, int page = 0)
            => new List<CampaignWithStats>
            {
                GetCampaignOne(),
                GetCampaignOne()
            };

        public async Task<IEnumerable<CampaignWithStats>> GetInactiveAsync(QueryCampaignWithStats query, int page = 0)
        {
            var result = new List<CampaignWithStats>();
            result.Add(GetCampaignTwo());
            return result;
        }

        public async Task<IEnumerable<CampaignWithStats>> GetPendingAsync(QueryCampaignWithStats query, int page = 0)
        {
            var result = new List<CampaignWithStats>();
            result.Add(GetCampaignTwo());
            result.Add(GetCampaignTwo());
            result.Add(GetCampaignTwo());
            return result;
        }

        public Task<IEnumerable<CampaignWithStats>> GetQueriedAsync(QueryCampaignWithStats query, int page = 0)
        {
            throw new NotImplementedException();
        }

        private CampaignWithStats GetCampaignOne() => new CampaignWithStats
        {
            Id = Guid.NewGuid(),
            Name = "Dummy campaign one",
            BrandingText = "Some branding text",
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
            EndDate = DateTime.Now,
            Publisher = Model.Enums.Publisher.Taboola,
            TargetUrl = "www.oznak.nl",
            Utm = "base_utm_code",
            Delivery = Model.Enums.Delivery.Balanced
        };

        private CampaignWithStats GetCampaignTwo() => new CampaignWithStats
        {
            Id = Guid.NewGuid(),
            Name = "Dummy campaign two",
            BrandingText = "Some branding text",
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
            EndDate = DateTime.Now,
            Publisher = Model.Enums.Publisher.Taboola,
            TargetUrl = "www.oznak.nl",
            Utm = "base_utm_code",
            Delivery = Model.Enums.Delivery.Strict
        };

        public Task<int> GetCount(QueryCampaignWithStats query)
            => Task.FromResult(new Random().Next(40, 350));
    }

}
