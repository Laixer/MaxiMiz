using Maximiz.Model.Protocol;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Core.Operations.Execution
{

    /// <summary>
    /// Contract for extracting <see cref="CreateOrUpdateObjectsMessage"/>s 
    /// from a given <see cref="Operation"/>.
    /// </summary>
    public interface IOperationMessagesExtractor
    {

        /// <summary>
        /// Extracts all <see cref="CreateOrUpdateObjectsMessage"/>s from the
        /// specified <paramref name="operation"/>. This creates a single 
        /// message for each entity in <see cref="Operation.Entities"/>.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="IEnumerable{CreateOrUpdateObjectsMessage}"/></returns>
        Task<IEnumerable<CreateOrUpdateObjectsMessage>> ExtractMessages(Operation operation);

        /// <summary>
        /// Extracts all <see cref="CreateOrUpdateObjectsMessage"/>s from the
        /// specified <paramref name="operation"/>. This creates a single 
        /// message for each entity in <see cref="Operation.EntitiesBeforeModification"/>.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="IEnumerable{CreateOrUpdateObjectsMessage}"/></returns>
        Task<IEnumerable<CreateOrUpdateObjectsMessage>> ExtractRollingBackMessagesAsync(Operation operation);

    }
}
