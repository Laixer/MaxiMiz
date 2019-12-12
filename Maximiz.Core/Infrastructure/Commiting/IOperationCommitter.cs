using Maximiz.Core.Operations;
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
        Task MarkAllAsPendingAsync(Operation operation);

        /// <summary>
        /// Marks all items in the <paramref name="operation"/> as processing in 
        /// our data store.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="Task"/></returns>
        Task MarkAllAsProcessingAsync(Operation operation);

        /// <summary>
        /// Markks all items in the <paramref name="operation"/> as up to date
        /// in our date store.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="Task"/></returns>
        Task MarkAllAsUpToDateAsync(Operation operation);

        /// <summary>
        /// Marks the <paramref name="operation"/> as exception reached state.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="Task"/></returns>
        Task MarkAsExceptionAsync(Operation operation);

        /// <summary>
        /// Marks the <paramref name="operation"/> as timed out state.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="Task"/></returns>
        Task MarkAsTimeoutAsync(Operation operation);

        /// <summary>
        /// Marks all items in the <paramref name="operation"/> as rolling
        /// back in our data store.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="Task"/></returns>
        Task MarkAllAsRollingBackAsync(Operation operation);

        Task MarkAllAsInOperationAsync(Operation operation);

    }
}
