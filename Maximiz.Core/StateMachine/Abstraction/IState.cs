using Maximiz.Core.Operations;
using System.Threading.Tasks;

namespace Maximiz.Core.StateMachine.Abstraction
{

    /// <summary>
    /// Contract for an executable state.
    /// </summary>
    public interface IState
    {

        /// <summary>
        /// Executes the functionality bound to this state.
        /// </summary>
        /// <returns><see cref="true"/> if successful</returns>
        Task<bool> ExecuteTransitionAsync(Operation operation);

        /// <summary>
        /// Undoes all that was done in <see cref="ExecuteTransitionAsync(Operation)"/>.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="true"/> if successful</returns>
        Task<bool> UndoTransitionAsync(Operation operation);

    }

}
