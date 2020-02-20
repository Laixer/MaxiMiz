using Maximiz.Core.Operations;
using Maximiz.Core.StateMachine.Abstraction;
using Maximiz.Model.Operations;
using System;
using System.Threading.Tasks;

namespace Maximiz.Core.StateMachine.States
{

    /// <summary>
    /// State in which our <see cref="Operation"/> is being rolled back. This state
    /// should never perform any functionality, since it can only be reached while
    /// we are still rolling back some of the items. When we are done we would reach
    /// the <see cref="MarkedPendingState"/>, which will handle further processing. 
    /// The last item that is being processed will automatically lead us into this state.
    /// </summary>
    public sealed class RollingBackState : IPassiveState
    {

        public int MaxExecuteAttempts => throw new InvalidOperationException($"{nameof(RollingBackState)} is passive");
        public int MaxUndoAttempts => throw new InvalidOperationException($"{nameof(RollingBackState)} is passive");
        public TimeSpan DelayExecuteAttempt => throw new InvalidOperationException($"{nameof(RollingBackState)} is passive");
        public TimeSpan DelayExecuteUndo => throw new InvalidOperationException($"{nameof(RollingBackState)} is passive");

        /// <summary>
        /// This will throw an <see cref="InvalidOperationException"/> because
        /// this state is passive.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="InvalidOperationException"/></returns>
        public Task<bool> ExecuteTransitionAsync(Operation operation)
            => throw new InvalidOperationException($"{nameof(RollingBackState)} is passive");

        /// <summary>
        /// This will throw an <see cref="InvalidOperationException"/> because
        /// this state is passive.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="InvalidOperationException"/></returns>
        public Task<bool> UndoTransitionAsync(Operation operation)
            => throw new InvalidOperationException($"{nameof(RollingBackState)} is passive");

    }
}
