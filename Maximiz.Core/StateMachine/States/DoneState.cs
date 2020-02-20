using Maximiz.Core.Operations;
using Maximiz.Core.StateMachine.Abstraction;
using Maximiz.Model.Operations;
using System;
using System.Threading.Tasks;

namespace Maximiz.Core.StateMachine.States
{

    /// <summary>
    /// State indicating we are done, also an <see cref="IFinalState"/>. This
    /// state should alwasy result in log success and exit, as it contains no
    /// functionality. Treating it any other way will result in an 
    /// <see cref="InvalidOperationException"/>.
    /// </summary>
    public sealed class DoneState : IFinalState
    {

        public int MaxExecuteAttempts => throw new InvalidOperationException("Final state should never be transitioned from");
        public int MaxUndoAttempts => throw new InvalidOperationException("Final state should never be transitioned from");
        public TimeSpan DelayExecuteAttempt => throw new InvalidOperationException("Final state should never be transitioned from");
        public TimeSpan DelayExecuteUndo => throw new InvalidOperationException("Final state should never be transitioned from");

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> because this state is final.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="InvalidOperationException"/></returns>
        public Task<bool> ExecuteTransitionAsync(Operation operation)
            => throw new InvalidOperationException("Final state should never be transitioned from");

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> because this state is final.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="InvalidOperationException"/></returns>
        public Task<bool> UndoTransitionAsync(Operation operation)
            => throw new InvalidOperationException("Final state should never be transitioned from");

    }

}
