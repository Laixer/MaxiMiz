using Maximiz.Core.Operations;
using System;
using System.Threading.Tasks;

namespace Maximiz.Core.StateMachine.Abstraction
{

    /// <summary>
    /// Contract for our finite state machine manager.
    /// </summary>
    public interface IStateManager
    {

        /// <summary>
        /// Attempts to launch our state machine for a given <see cref="Operation"/>.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="Task"/></returns>
        Task AttemptStartStateMachineAsync(Operation operation);

        /// <summary>
        /// Attempts to read the state of an <see cref="Operation"/> and act on it if required.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="Task"/></returns>
        Task AttemptContinueStateMachineAsync(Operation operation);

    }
}
