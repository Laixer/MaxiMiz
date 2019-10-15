using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Abstraction
{
    public interface IAdItemRepository : IEntityRepository<AdItem, Guid>
    {
        // TODO Temporary
        /// <summary>
        /// Returns ads
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AdItem>> GetAds();
    }
}
