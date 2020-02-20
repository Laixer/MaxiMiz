using Maximiz.Model.Operations;
using Maximiz.Model.Protocol;
using System;

namespace Maximiz.Core.Utility
{

    /// <summary>
    /// Functionality for extracting an <see cref="OperationMessage"/> from an <see cref="MyOperation"/>.
    /// </summary>
    public static class MessageExtractor
    {

        /// <summary>
        /// Extracts a <see cref="OperationMessage"/> from an <see cref="MyOperation"/>.
        /// </summary>
        /// <param name="operation"><see cref="MyOperation"/></param>
        /// <returns><see cref="OperationMessage"/></returns>
        public static OperationMessage Extract(MyOperation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            return new OperationMessage(operation.TopEntity, operation.CrudAction, operation.Id);
        }

    }
}
