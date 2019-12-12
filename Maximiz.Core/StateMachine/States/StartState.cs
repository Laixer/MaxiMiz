using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Core.Operations;
using Maximiz.Core.Operations.Execution;
using Maximiz.Core.StateMachine.Abstraction;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Maximiz.Core.StateMachine.States
{

    /// <summary>
    /// Starting state, where our <see cref="Operation"/> has been created but
    /// nothing else has happened yet. 
    /// </summary>
    public sealed class StartState : IStartState
    {

        public int MaxExecuteAttempts => 25;
        public int MaxUndoAttempts => 25;
        public TimeSpan DelayExecuteAttempt => TimeSpan.FromSeconds(15);
        public TimeSpan DelayExecuteUndo => TimeSpan.FromSeconds(15);

        private readonly IOperationCommitter _operationCommitter;
        private readonly IOperationHistoryCreator _operationHistoryCreator;
        private readonly ILogger logger;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public StartState(IOperationCommitter operationCommitter,
            IOperationHistoryCreator operationHistoryCreator,
            ILoggerFactory loggerFactory)
        {
            _operationCommitter = operationCommitter ?? throw new ArgumentNullException(nameof(operationCommitter));
            _operationHistoryCreator = operationHistoryCreator ?? throw new ArgumentNullException(nameof(operationHistoryCreator));

            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            logger = loggerFactory.CreateLogger(nameof(StartState));
        }

        /// <summary>
        /// This populates the backup table with a before and after for each 
        /// item in our <paramref name="operation"/>.
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
                    await _operationCommitter.MarkAllAsInOperationAsync(operation);
                    await _operationHistoryCreator.CreateHistoryAsync(operation);
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
        /// Removes all history for our <paramref name="operation"/> and sets 
        /// all its items to up to date.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="true"/> if successful</returns>
        public async Task<bool> UndoTransitionAsync(Operation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _operationHistoryCreator.DeleteHistoryAsync(operation);
                    await _operationCommitter.MarkAllAsUpToDateAsync(operation);
                    transactionScope.Complete();
                    return true;
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Transition undo transaction failed");
                    return false;
                }
            }
        }

    }

}
