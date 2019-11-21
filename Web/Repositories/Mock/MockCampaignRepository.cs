using Maximiz.Database.Querying;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Maximiz.Model.Enums;

namespace Maximiz.Repositories.Mock
{

    /// <summary>
    /// Contains mock functions to return lists.
    /// </summary>
    public class MockCampaignRepository : ICampaignRepository
    {
        private Random random = new Random();

        public async Task<CampaignWithStats> Get(Guid id) => GetCampaignOne();

        public async Task<IEnumerable<CampaignWithStats>> GetAllAsync(QueryCampaignWithStats query, int page = 0)
            => GenerateRandom(random.Next(4, 25));

        public async Task<IEnumerable<CampaignWithStats>> GetActiveAsync(QueryCampaignWithStats query, int page = 0)
            => GenerateRandom(random.Next(4, 25));

        public async Task<IEnumerable<CampaignWithStats>> GetInactiveAsync(QueryCampaignWithStats query, int page = 0)
            => GenerateRandom(random.Next(4, 25));

        public async Task<IEnumerable<CampaignWithStats>> GetPendingAsync(QueryCampaignWithStats query, int page = 0)
            => GenerateRandom(random.Next(4, 25));

        public async Task<IEnumerable<CampaignWithStats>> GetQueriedAsync(QueryCampaignWithStats query, int page = 0)
            => GenerateRandom(random.Next(4, 25));

        public Task<int> GetCount(QueryCampaignWithStats query)
            => Task.FromResult(new Random().Next(40, 350));

        private IEnumerable<CampaignWithStats> GenerateRandom(int total)
        {
            var result = new List<CampaignWithStats>();
            for (int i = 0; i < total; i++)
            {
                result.Add(GetCampaignOne());
            }
            return result;
        }

        private CampaignWithStats GetCampaignOne() => new CampaignWithStats
        {
            // Main
            Id = Guid.NewGuid(),

            // Stats
            Spent = random.Next(100, 1000),
            Clicks = random.Next(10000),
            Roi = random.NextDouble()*100,
            Revenue = random.Next(0, 10000),
            RevenueAdsense = random.Next(0, 5000),
            RevenueTaboola = random.Next(0, 5000),
            Profit = random.Next(0, 3000),
            Actions = random.Next(10000),

            // Account
            Publisher = Publisher.Taboola,
            Name = "Dummy campaign",
            BrandingText = "Some branding text",
            Utm = "base_utm_code",
            TargetUrl = "https://www.laixer.com",

            // Marketing
            Budget = random.Next(30, 3000),
            BudgetDaily = random.Next(0, 100),
            BidStrategy = BidStrategy.Smart,
            Delivery = Delivery.Balanced,
            InitialCpc = (decimal)random.NextDouble()*5,

            // Targeting
            Devices = new[] { Device.Desktop },
            OperatingSystems = new[] { OS.Android },
            LocationInclude = new[] { 1 },

            // Schedule
            StartDate = DateTime.Now,
            EndDate = DateTime.Now,
        };

    }

}
