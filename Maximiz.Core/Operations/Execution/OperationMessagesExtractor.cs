using Maximiz.Model.Entity;
using Maximiz.Model.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maximiz.Core.Operations.Execution
{

    /// <summary>
    /// Extracts 
    /// </summary>
    public sealed class OperationMessagesExtractor
    {

        /// <summary>
        /// Extracts all <see cref="CreateOrUpdateObjectsMessage"/>s from the
        /// specified <paramref name="operation"/>. This creates a single 
        /// message for each entity in <see cref="Operation.Entities"/>.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="IEnumerable{CreateOrUpdateObjectsMessage}"/></returns>
        public IEnumerable<CreateOrUpdateObjectsMessage> Extract(Operation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }
            return operation.Entities.Select(x => ExtractSingle(x)).ToList();
        }

        /// <summary>
        /// Creates a single <see cref="CreateOrUpdateObjectsMessage"/> based on
        /// the specified <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/></param>
        /// <returns><see cref="CreateOrUpdateObjectsMessage"/></returns>
        private CreateOrUpdateObjectsMessage ExtractSingle(Entity entity)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }

            throw new NotImplementedException();
        }

    }
}
