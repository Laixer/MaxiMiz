using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Core.Operations;
using Maximiz.Core.StateMachine.Abstraction;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Maximiz.Core.StateMachine.States
{

    /// <summary>
    /// State in which all our operations succeeded.
    /// </summary>
    public sealed class SuccessState : IState
    {

        public int MaxExecuteAttempts => 25;
        public int MaxUndoAttempts => 1;
        public TimeSpan DelayExecuteAttempt => TimeSpan.FromSeconds(30);
        public TimeSpan DelayExecuteUndo => TimeSpan.FromSeconds(1);

        private readonly IOperationCommitter _operationCommitter;
        private readonly ILogger logger;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public SuccessState(IOperationCommitter operationCommitter, ILoggerFactory loggerFactory)
        {
            _operationCommitter = operationCommitter ?? throw new ArgumentNullException(nameof(operationCommitter));

            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            logger = loggerFactory.CreateLogger(nameof(SuccessState));
        }

        /// <summary>
        /// Marks all items in our <paramref name="operation"/> as up to date,
        /// then closes the operation.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="true"/> if successful</returns>
        public async Task<bool> ExecuteTransitionAsync(Operation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _operationCommitter.MarkAllAsUpToDateAsync(operation);
                    await _operationCommitter.CloseOperationAsync(operation, true); // TODO Two separate calls?
                    transactionScope.Complete();
                    return true;
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Transition transaction failed");
                    return false;
                }
            }
        }

        /// <summary>
        /// This undo transition should never be reached since a database transaction
        /// is its own undo.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="InvalidOperationException"/></returns>
        public Task<bool> UndoTransitionAsync(Operation operation)
            => throw new InvalidOperationException($"The undo of our success " +
                $"state should never be reached since transactions are its own undos");
    }
}
