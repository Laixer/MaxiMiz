using Maximiz.InputModels;
using Maximiz.Model.Entity;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Abstraction
{ 

    /// <summary>
    /// Abstraction for an <see cref="AdGroup"/> repository.
    /// </summary>
    public interface IAdGroupRepository : IEntityRepository<AdGroup, int>
    {

        /// <summary>
        /// Create a new <see cref="AdGroup"/> with <see cref="AdItem"/>s.
        /// </summary>
        /// <param name="adGroupInputModel">The model containing the input for the ad group.</param>
        Task CreateGroup(AdGroupInputModel adGroupInputModel);

    }
}
