using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Core.Operations;
using Maximiz.Core.Operations.Execution;
using Maximiz.Core.StateMachine.Abstraction;
using Maximiz.Model.Operations;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Maximiz.Core.StateMachine
{

    /// <summary>
    /// Manages our <see cref="IState"/> transitions.
    /// </summary>
    public sealed class StateManager : IStateManager
    {

        private readonly IStateReader _stateReader;
        private readonly ILogger logger;
        private readonly IOperationCommitter _operationCommitter;
        private readonly EntityAvailabilityChecker _entityAvailabilityChecker;
        private readonly IOperationHistoryCreator _operationHistoryCreator;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public StateManager(IStateReader stateReader, ILoggerFactory loggerFactory,
            IOperationCommitter operationCommitter, EntityAvailabilityChecker entityAvailabilityChecker,
            IOperationHistoryCreator operationHistoryCreator)
        {
            _stateReader = stateReader ?? throw new ArgumentNullException(nameof(stateReader));
            _operationCommitter = operationCommitter ?? throw new ArgumentNullException(nameof(operationCommitter));
            _entityAvailabilityChecker = entityAvailabilityChecker ?? throw new ArgumentNullException(nameof(entityAvailabilityChecker));
            _operationHistoryCreator = operationHistoryCreator ?? throw new ArgumentNullException(nameof(operationHistoryCreator));

            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            logger = loggerFactory.CreateLogger(nameof(StateManager));
        }

        /// <summary>
        /// Attempts to start the state machine.
        /// </summary>
        /// <remarks>
        /// This will also trigger the execution of the successive states in an
        /// async void method. TODO Should we do this? How else to solve this?
        /// </remarks>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="true"/> if successful</returns>
        public async Task<bool> AttemptStartStateMachineAsync(EntityMap entityMap)
        {
            if (entityMap == null) { throw new ArgumentNullException(nameof(entityMap)); }

            if (!await _entityAvailabilityChecker.AreEntitiesAvailable(entityMap.Entities))
            {
                logger.LogTrace("Requested entities for operation are not available");
                return false;
            }

            var operation = new Operation
            {
                EntityIds = entityMap.Entities.Select(x => x.Id)
            };

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (var source = new CancellationTokenSource())
            {
                try
                {
                    await _operationCommitter.CreateAsync(operation, source.Token); // Only affects operation table
                    await _operationCommitter.MarkAllAsInOperationAsync(operation); // Only affects entity tables
                    await _operationHistoryCreator.CreateHistoryAsync(operation); // Only affects history tables

                    // Check again to prevent race conditions
                    // TODO Do we have to?
                    if (!await _entityAvailabilityChecker.AreEntitiesAvailable(entityMap.Entities))
                    {
                        logger.LogTrace("Requested entities for operation are not available");
                        return false;
                    }

                    // Commit the transaction
                    transactionScope.Complete();
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Could not create and start operation");
                    return false;
                }
            }

            // If we reach this point we are ready to go
            // This has to exist outside of the transaction because it must be committed before we can start
            // TODO Is this the right way to launch our state machine?
            var thread = new Thread(async () => await AttemptContinueStateMachineAsync(operation));
            logger.LogTrace($"Launching operation {operation.Id} in thread {thread.ManagedThreadId}");
            return true;
        }

        /// <summary>
        /// Reads the current state and transitions if required. This will also
        /// attempt a transition until it succeeds or until the maximum amount 
        /// of attempts is exceeded. It will undo the transition if we can't make
        /// it.
        /// </summary>
        /// <remarks>
        /// This entire function exists within a try catch block. If anything 
        /// within our state machine fails and is not caught inside of the internal
        /// operations, we will be directed to the <see cref="OnExceptionReached"/>
        /// function for a log & exit procedure.
        /// </remarks>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="Task"/></returns>
        public async Task AttemptContinueStateMachineAsync(Operation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            try
            {
                // Read the state to know where we are in the state machine
                var state = await _stateReader.GetCurrentStateAsync(operation);

                // Stop the state machine if we are in a final state.
                if (state is IFinalState)
                {
                    logger.LogTrace($"Completed operation id = {operation.Id} in state {nameof(state)}");
                    return;
                }

                // Do nothing if the state is passive
                if (state is IPassiveState)
                {
                    logger.LogTrace($"Doing nothing in passive state {nameof(state)} for operation id = {operation.Id}");
                    return;
                }

                // Do the actual transition
                var transitionSuccess = await DriveStateTransitionAsync(state, operation);

                // Handle if we should call our next state transition or not
                if (transitionSuccess)
                {
                    if (state is IDontChainState)
                    {
                        logger.LogTrace($"Stopping chain state execution in state {nameof(state)}");
                        return;
                    }
                    else
                    {
                        await AttemptContinueStateMachineAsync(operation);
                    }
                }
                else
                {
                    // If we reach this we can't perform our state transition, but we were atomic.
                    // This part is reached after all transition attempts AND all undo attempts.
                    var exception = new InvalidOperationException($"Could not make state transition " +
                        $"for state {nameof(state)}, even though all operations were atomic.");
                    await OnExceptionReached(exception, operation);
                }
            }
            catch (Exception e)
            {
                await OnExceptionReached(e, operation);
            }
        }

        /// <summary>
        /// Drives a state transition, including its undo if required.
        /// </summary>
        /// <remarks>
        /// This throws an <see cref="InvalidOperationException"/> if our state
        /// transition is not atomic for some reason (see log).
        /// </remarks>
        /// <param name="state"><see cref="IState"/></param>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="true"/> if successful</returns>
        private async Task<bool> DriveStateTransitionAsync(IState state, Operation operation)
        {
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            // Attempt to transition
            for (int i = 0; i < state.MaxExecuteAttempts; i++)
            {
                try
                {
                    if (await state.ExecuteTransitionAsync(operation))
                    {
                        logger.LogTrace($"Successful transition attempt {i + 1} for state {nameof(state)}");
                        return true;
                    }
                    else
                    {
                        logger.LogTrace($"Failed at transition attempt {i + 1}/{state.MaxExecuteAttempts} for state {nameof(state)}");
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message, $"Error in transition attempt {i + 1}/{state.MaxExecuteAttempts} for state {nameof(state)}");
                }

                // Wait before trying again
                await Task.Delay(state.DelayExecuteAttempt);
            }

            // If we reach this point we have failed doing our transition
            logger.LogError($"Could not perform transition for {nameof(state)} after {state.MaxExecuteAttempts} attempts. Going to undo phase");

            // Attempt to undo
            for (int i = 0; i < state.MaxUndoAttempts; i++)
            {
                try
                {
                    if (await state.UndoTransitionAsync(operation))
                    {
                        logger.LogTrace($"Successful transition undo attempt {i + 1} for state {nameof(state)}");
                        return false;
                    }
                    else
                    {
                        logger.LogTrace($"Failed transition undo attempt {i + 1}/{state.MaxUndoAttempts} for state {nameof(state)}");
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message, $"Error in transition undo attempt {i + 1}/{state.MaxUndoAttempts} for state {nameof(state)}");
                }

                // Wait before trying again
                await Task.Delay(state.DelayExecuteUndo);
            }

            // If we reach this point we couldn't undo our transition,
            // Thus we don't know how to recover. We throw an exception.
            throw new InvalidOperationException($"Unable to complete atomic state operation for {nameof(state)} for operation id = {operation.Id}");
        }

        /// <summary>
        /// When our state machine reaches an exception, there is nothing we 
        /// can do but log and exit. This also attempts to update the data store
        /// with the exception state, but this might fail. This is logged as well.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns>Nothing</returns>
        private async Task OnExceptionReached(Exception exception, Operation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            logger.LogError(exception, $"Exception state reached in operation id = {operation.Id}. " +
                $"Attempting to update database, then log & exit will occur");

            // Attempt to update database, mark as exception reached
            for (int i = 0; i < 25; i++)
            {
                try
                {
                    await _operationCommitter.MarkAsExceptionAsync(operation);
                    logger.LogTrace($"Operation {operation.Id} marked as exception reached");
                    break;
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Error while marking operation {operation.Id} as exception reached");
                }

                // Wait for the next trt
                await Task.Delay(TimeSpan.FromSeconds(20));
            }

            // Log and exit, there's nothing we can do
            logger.LogError($"Log & exit occuring now for operation id = {operation.Id}");
            return;
        }

        public Task AttemptStartStateMachineAsync(MyOperation operation)
        {
            throw new NotImplementedException();
        }
    }

}
