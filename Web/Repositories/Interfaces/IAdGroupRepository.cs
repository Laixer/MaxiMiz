using Maximiz.InputModels;
using Maximiz.Model.Entity;
using System;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Interfaces
{
    public interface IAdGroupRepository : IEntityRepository<AdGroup, int>
    {
        /// <summary>
        /// Create a new AdGroup
        /// </summary>
        /// <param name="inputModel">The model containing the input for the ad</param>
        /// <returns></returns>
        Task CreateGroup(AdGroupInputModel inputModel);
    }
}
