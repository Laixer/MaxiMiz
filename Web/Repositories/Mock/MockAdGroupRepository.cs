using Maximiz.Database.Querying;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Mock
{
    public class MockAdGroupRepository : IAdGroupRepository
    {
        // Initialize random object
        private Random random = new Random();

        // Re-use the same GUIDS throughout the entire process
        private static List<Guid> guids = new List<Guid>();
        static MockAdGroupRepository()
        {
            for (int i = 0; i < 50; i++)
            {
                guids.Add(Guid.NewGuid());
            }
        }

        public async Task<AdGroupWithStats> Get(Guid id)
            => AdGroupOne(0);

        public async Task<IEnumerable<AdGroupWithStats>> GetAllAsync(QueryAdGroupWithStats query, int page = 0)
            => GenerateRandom(random.Next(5, 25));

        public Task<int> GetCount(QueryAdGroupWithStats query)
            => Task.FromResult(random.Next(20, 35));

        public async Task<IEnumerable<AdGroupWithStats>> GetLinkedWithCampaignAsync(Guid idCampaign, QueryAdGroupWithStats query)
            => GenerateRandom(random.Next(2, 5));

        public async Task<IEnumerable<AdGroupWithStats>> GetQueriedAsync(QueryAdGroupWithStats query, int page = 0)
            => GenerateRandom(random.Next(5, 25));

        private IEnumerable<AdGroupWithStats> GenerateRandom(int total)
        {
            var result = new List<AdGroupWithStats>();
            if (total > guids.Count) { total = guids.Count; }
            for (int i = 0; i < total; i++)
            {
                result.Add(AdGroupOne(i));
            }
            return result;
        }

        private AdGroupWithStats AdGroupOne(int idIndex)
        {
            return new AdGroupWithStats()
            {
                Id = guids[idIndex],
                Name = "Dummy Ad Group",
                Description = "A lovely adgroup",
                ImageLinks = new[] { "link.one", "link.two", "link.three" },
                Titles = new[] { "Image one", "Image two", "Image three" },
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                AdItemCount = random.Next(3, 40)
            };
        }
    }
}
