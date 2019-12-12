using Maximiz.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Core.Operations.Abstraction
{

    /// <summary>
    /// Contract for creating a <see cref="Operation"/> under the right circumstances.
    /// This is not responsible for starting said operation.
    /// </summary>
    public interface IOperationManager
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitiesModified"><see cref="Entity"/></param>
        /// <returns><see cref="Operation"/></returns>
        Task<Operation> CreateOperationAsync(IEnumerable<Entity> entitiesModified);

    }

}
