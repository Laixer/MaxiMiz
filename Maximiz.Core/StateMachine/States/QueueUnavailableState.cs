using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Core.Operations;
using Maximiz.Core.StateMachine.Abstraction;
using System;
using System.Threading.Tasks;

namespace Maximiz.Core.StateMachine.States
{

    /// <summary>
    /// State indicating our queue is unavailable. This marks all items in the 
    /// database back to up-to-date from pending.
    /// </summary>
    public sealed class QueueUnavailableState : IState
    {

        public int MaxExecuteAttempts => 25;
        public int MaxUndoAttempts => throw new InvalidOperationException($"State {nameof(QueueUnavailableState)} can't be undone");
        public TimeSpan DelayExecuteAttempt => TimeSpan.FromSeconds(15);
        public TimeSpan DelayExecuteUndo => throw new InvalidOperationException($"State {nameof(QueueUnavailableState)} can't be undone");

        private readonly IOperationCommitter _operationCommitter;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public QueueUnavailableState(IOperationCommitter operationCommitter)
        {
            _operationCommitter = operationCommitter ?? throw new ArgumentNullException(nameof(operationCommitter));
        }

        /// <summary>
        /// Marks all items in our <paramref name="operation"/> as up to date.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="true"/></returns>
        public async Task<bool> ExecuteTransitionAsync(Operation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            await _operationCommitter.MarkAllAsUpToDateAsync(operation);
            return true;
        }

        /// <summary>
        /// This undo operation should never happen, thus it throws an exception.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="InvalidOperationException"/></returns>
        public Task<bool> UndoTransitionAsync(Operation operation)
            => throw new InvalidOperationException($"State {nameof(QueueUnavailableState)} can't be undone");
    }
}
