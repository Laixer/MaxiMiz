using Maximiz.Core.Operations;
using System;
using System.Threading.Tasks;

namespace Maximiz.Core.StateMachine.Abstraction
{

    /// <summary>
    /// Contract for an executable state.
    /// </summary>
    public interface IState
    {

        /// <summary>
        /// Indicates how many state transition attempts we are allowed to do.
        /// </summary>
        int MaxExecuteAttempts { get; }

        /// <summary>
        /// Indicates how many state transition undo attempts we are allowed to do.
        /// </summary>
        int MaxUndoAttempts { get; }

        /// <summary>
        /// Indicates how long we should wait between two transition attempts.
        /// </summary>
        TimeSpan DelayExecuteAttempt { get; }

        /// <summary>
        /// Indicates how long we should wait between two transition undo attempts.
        /// </summary>
        TimeSpan DelayExecuteUndo { get; }

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
