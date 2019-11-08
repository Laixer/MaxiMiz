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
        public async Task<AdGroupWithStats> Get(Guid id)
            => AdGroupOne();

        public async Task<IEnumerable<AdGroupWithStats>> GetAllAsync(QueryAdGroupWithStats query, int page = 0)
            => new List<AdGroupWithStats> { AdGroupOne(), AdGroupTwo(), AdGroupOne(), AdGroupTwo(), AdGroupTwo() , AdGroupOne(), AdGroupTwo(), AdGroupOne(), AdGroupTwo(), AdGroupTwo() };

        public Task<int> GetCount(QueryAdGroupWithStats query)
            => Task.FromResult(new Random().Next(23, 1875));

        public async Task<IEnumerable<AdGroupWithStats>> GetLinkedWithCampaignAsync(Guid idCampaign, QueryAdGroupWithStats query)
        {
            await Task.Delay(500);
            return new List<AdGroupWithStats> { AdGroupOne(), AdGroupTwo() };
        }

        public Task<IEnumerable<AdGroupWithStats>> GetQueriedAsync(QueryAdGroupWithStats query, int page = 0)
        {
            throw new NotImplementedException();
        }

        private AdGroupWithStats AdGroupOne()
        {
            return new AdGroupWithStats()
            {
                Id = Guid.NewGuid(),
                Name = "AdGroup A",
                Description = "A lovely adgroup",
                ImageLinks = new[] { "link.one", "link.two", "link.three" },
                Titles = new[] { "Image one", "Image two", "Image three" },
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                AdItemCount = new Random().Next(3, 40)
            };
        }

        private AdGroupWithStats AdGroupTwo()
        {
            return new AdGroupWithStats()
            {
                Id = Guid.NewGuid(),
                Name = "AdGroup B",
                Description = "A lovely adgroup",
                ImageLinks = new[] { "link.one", "link.two", "link.three" },
                Titles = new[] { "Image one", "Image two", "Image three" },
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                AdItemCount = new Random().Next(3, 40)
            };
        }
    }
}
