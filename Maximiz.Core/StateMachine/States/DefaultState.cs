using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Core.Operations;
using Maximiz.Core.StateMachine.Abstraction;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Maximiz.Core.StateMachine.States
{

    /// <summary>
    /// The default state.
    /// </summary>
    public sealed class DefaultState : IState
    {

        public int MaxExecuteAttempts => 25;
        public int MaxUndoAttempts => 25;
        public TimeSpan DelayExecuteAttempt => TimeSpan.FromSeconds(90);
        public TimeSpan DelayExecuteUndo => TimeSpan.FromSeconds(90);

        private ILogger logger;
        private IOperationCommitter _operationCommitter;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public DefaultState(IOperationCommitter operationCommitter, ILoggerFactory loggerFactory)
        {
            _operationCommitter = operationCommitter ?? throw new ArgumentNullException(nameof(operationCommitter));
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            logger = loggerFactory.CreateLogger(nameof(DefaultState));
        }

        /// <summary>
        /// Sets all items in the <paramref name="operation"/> to pending in our
        /// data store.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="true"/> if succeeded</returns>
        public async Task<bool> ExecuteTransitionAsync(Operation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            await _operationCommitter.MarkAllAsPendingAsync(operation);
            return true;
        }

        /// <summary>
        /// Marks all items back to in operation.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="true"/> if successful</returns>
        public async Task<bool> UndoTransitionAsync(Operation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            await _operationCommitter.MarkAllAsInOperationAsync(operation);
            return true;
        }

    }

}
