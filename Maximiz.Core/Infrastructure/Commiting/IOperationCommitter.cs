using Maximiz.Core.Operations;
using System;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Commiting
{

    /// <summary>
    /// Contract for committing <see cref="Operation"/>s to our data store.
    /// </summary>
    public interface IOperationCommitter : ICommitter<Operation>
    {

        /// <summary>
        /// Marks an <see cref="Operation"/> as closed.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <param name="success">bool indicating success</param>
        /// <returns><see cref="Task"/></returns>
        Task CloseOperationAsync(Operation operation, bool success);

        /// <summary>
        /// Marks all items in the <paramref name="operation"/> as pending in 
        /// our data store.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="Task"/></returns>
        Task MarkAllAsPending(Operation operation);

        /// <summary>
        /// Marks all items in the <paramref name="operation"/> as processing in 
        /// our data store.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="Task"/></returns>
        Task MarkAllAsProcessing(Operation operation);

    }
}
