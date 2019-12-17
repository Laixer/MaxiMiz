using Maximiz.Model.Operations;
using System.Threading.Tasks;

namespace Maximiz.Core.Operations.Execution
{

    /// <summary>
    /// Contract for creating the before and after variants of each item in our
    /// <see cref="Operation"/>.
    /// </summary>
    public interface IOperationHistoryCreator
    {

        /// <summary>
        /// Extracts a before and after copy of each item in our <paramref name="operation"/>.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="Task"/></returns>
        Task CreateHistoryAsync(Operation operation);

        /// <summary>
        /// Deletes all created item copies for our <paramref name="operation"/>.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="Task"/></returns>
        Task DeleteHistoryAsync(Operation operation);

    }
}
