using Maximiz.Core.Operations;
using Maximiz.Core.StateMachine.Abstraction;
using System;
using System.Threading.Tasks;

namespace Maximiz.Core.StateMachine.States
{

    /// <summary>
    /// State indicating we are timed out, also an <see cref="IFinalState"/>.
    /// <see cref="InvalidOperationException"/>.
    /// </summary>
    public sealed class TimeoutState : IFinalState
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
