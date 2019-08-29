using Maximiz.InputModels;
using Maximiz.Model.Entity;
using System;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Interfaces
{
    public interface IAdGroupRepository : IEntityRepository<AdGroup, int>
    {
        /// <summary>
        /// Create a new Ad Group with Ad Items.
        /// </summary>
        /// <param name="adGroupInput">The model containing the input for the ad</param>
        Task CreateGroup(AdGroupInputModel adGroupInput);
    }
}
