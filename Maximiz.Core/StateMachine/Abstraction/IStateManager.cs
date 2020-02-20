using Maximiz.Model.Operations;
using System.Threading.Tasks;

namespace Maximiz.Core.StateMachine.Abstraction
{

    /// <summary>
    /// Contract for our finite state machine manager.
    /// </summary>
    public interface IStateManager
    {

        /// <summary>
        /// Attempts to create a new <see cref="Operation"/> based on some <see cref="EntityMap"/>.
        /// </summary>
        /// <param name="entityMap"><see cref="EntityMap"/></param>
        /// <returns><see cref="true"/> if successful</returns>
        Task<bool> AttemptStartStateMachineAsync(EntityMap entityMap);

        /// <summary>
        /// Attempts to read the state of an <see cref="Operation"/> and act on it if required.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="Task"/></returns>
        Task AttemptContinueStateMachineAsync(Operation operation);

    }
}
