using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Core.Operations;
using Maximiz.Core.StateMachine.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maximiz.Core.StateMachine.States
{

    /// <summary>
    /// State indicating we have reached our timeout.
    /// TODO This should be implemented with a bunch of cancellations?
    /// </summary>
    public sealed class TimeoutReachedState : IState
    {

        public int MaxExecuteAttempts => 25;
        public int MaxUndoAttempts => throw new InvalidOperationException($"State {nameof(TimeoutReachedState)} can't be undone");
        public TimeSpan DelayExecuteAttempt => TimeSpan.FromSeconds(15);
        public TimeSpan DelayExecuteUndo => throw new InvalidOperationException($"State {nameof(TimeoutReachedState)} can't be undone");

        private readonly IOperationCommitter _operationCommitter;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public TimeoutReachedState(IOperationCommitter operationCommitter)
        {
            _operationCommitter = operationCommitter ?? throw new ArgumentNullException(nameof(operationCommitter));
        }

        /// <summary>
        /// Marks our <paramref name="operation"/> as timed out.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="true"/> if successful</returns>
        public async Task<bool> ExecuteTransitionAsync(Operation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            await _operationCommitter.MarkAsTimeoutAsync(operation);
            return true;
        }

        /// <summary>
        /// Database transactions are atomic and thus can't be undone in our 
        /// implementation, this will throw an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="InvalidOperationException"/></returns>
        public Task<bool> UndoTransitionAsync(Operation operation)
         => throw new InvalidOperationException($"State {nameof(TimeoutReachedState)} can't be undone");
    }
}
