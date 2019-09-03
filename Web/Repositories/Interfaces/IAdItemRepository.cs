using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Interfaces
{
    public interface IAdItemRepository : IEntityRepository<AdItem, Guid>
    {
        Task<IEnumerable<AdItem>> GetAds();
    }
}
