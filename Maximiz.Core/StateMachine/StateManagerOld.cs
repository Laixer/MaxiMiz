//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using Maximiz.Core.Infrastructure.Commiting;
//using Maximiz.Core.Infrastructure.Repositories;
//using Maximiz.Core.Operations;
//using Maximiz.Core.StateMachine.Abstraction;
//using Maximiz.Core.StateMachine.States;
//using Microsoft.Extensions.Logging;

//namespace Maximiz.Core.StateMachine
//{

//    /// <summary>
//    /// Processes a given <see cref="IState"/> and <see cref="Operation"/>. This
//    /// can be called from anywhere within the processing pipeline.
//    /// </summary>
//    public sealed class StateManagerOld : IStateManager
//    {

//        private ILogger logger;
//        private IOperationRepository _operationRepository;
//        private IOperationCommitter _operationCommitter;
//        private IStateReader _stateReader;

//        /// <summary>
//        /// This is used as a unique lock id for handling <see cref="Operation"/>s.
//        /// </summary>
//        private readonly Guid lockId = Guid.NewGuid();

//        /// <summary>
//        /// Construct for dependency injection.
//        /// </summary>
//        public StateManagerOld(ILoggerFactory loggerFactory, IOperationRepository operationRepository,
//            IOperationCommitter operationCommitter, IStateReader stateReader)
//        {
//            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
//            logger = loggerFactory.CreateLogger(nameof(StateManagerOld));

//            if (operationRepository == null) { throw new ArgumentNullException(nameof(operationRepository)); }
//            _operationRepository = operationRepository;

//            if (operationCommitter == null) { throw new ArgumentNullException(nameof(operationCommitter)); }
//            _operationCommitter = operationCommitter;

//            if (stateReader == null) { throw new ArgumentNullException(nameof(stateReader)); }
//            _stateReader = stateReader;
//        }

//        /// <summary>
//        /// Attempts to launch our state machine, entering in the first state and 
//        /// thus starting the entire mechanism.
//        /// </summary>
//        /// <param name="operation"><see cref="Operation"/></param>
//        /// <returns><see cref="Task"/></returns>
//        public async Task AttemptStartStateMachineAsync(Operation operation)
//        {
//            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

//            // TODO More state checks here
//            // Does thing already exist? --> exception
//            // Is the thing already locked? --> do nothing / exception

//            // TODO Timer launch
//            await AttemptContinueStateMachineAsync(operation, lockId); // This should get us in the initialization state.
//        }


//        /// <summary>
//        /// Gets the current state of an operation, checks if this is allowed
//        /// to perform state transitions and executes them if required.
//        /// </summary>
//        /// <param name="operation"><see cref="Operation"/></param>
//        /// <param name="lockId">The lock <see cref="Guid"/></param>
//        /// <returns><see cref="Task"/></returns>
//        public async Task AttemptContinueStateMachineAsync(Operation operation, Guid lockId)
//        {
//            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

//            try
//            {
//                // If we aren't locked, we should lock
//                if (!await _operationRepository.IsOperationLockedAsync(operation))
//                {
//                    await _operationCommitter.LockOperationAsync(operation, lockId);
//                    logger.LogTrace($"Operation {operation.Id} has been locked with {lockId}");

//                    var state = await _stateReader.GetCurrentStateAsync(operation);
//                    await ProcessStateAsync(operation);
//                }

//                // If we aren't the owner of the lock, we should exit
//                else if (!await _operationRepository.IsOperationLockedByIdAsync(operation, lockId))
//                {
//                    logger.LogTrace($"State manager does not have matchin lock id for operation {operation.Id}");
//                    return; // Maybe log this?
//                }
//            }
//            catch (Exception e)
//            {
//                // TODO Handle
//                // Could not perform state transaction. Big error. 
//                throw new NotImplementedException();
//            }
//        }

//        /// <summary>
//        /// Processes a derived <see cref="IState"/> for a given <see cref="Operation"/>.
//        /// </summary>
//        /// <param name="operation"><see cref="Operation"/></param>
//        /// <returns><see cref="Task"/></returns>
//        private async Task ProcessStateAsync(Operation operation)
//        {
//            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }
//            if (operation.Id == null) { throw new ArgumentNullException(nameof(operation.Id)); }
//            if (operation.Entities == null) { throw new ArgumentNullException(nameof(operation.Entities)); }
//            if (operation.StartDate == null) { throw new ArgumentNullException(nameof(operation.StartDate)); }

//            try
//            {
//                var state = await _stateReader.GetCurrentStateAsync(operation);
//                logger.LogTrace($"Processing state {state.ToString()} for operation id = {operation.Id}");

//                var result = await state.ExecuteTransitionAsync(operation);

//                // We stop if this state is final
//                if (state is IFinalState)
//                {
//                    logger.LogTrace($"Final state {state.ToString()} execution success" +
//                        $" = {result.Success} for operation id = {operation.Id}. Exiting.");
//                    return;
//                }

//                logger.LogTrace($"State {state.ToString()} execution success = {result.Success} " +
//                    $"for operation id = {operation.Id}, next state is {result.NextState.ToString()}");

//                // Only trigger the next state if this is required implicitly.
//                if (state is IContinueExplicitlyState)
//                {
//                    await ProcessStateAsync(operation);
//                }
//            }
//            catch (Exception e)
//            {
//                logger.LogError(e.Message, $"Error while processing state {state.ToString()} for operation id = {operation.Id}");
//                // TODO What we do on massive failure is defined here
//                throw new Exception("The state machine failed, implement some kind of handler");
//            }
//        }

//    }

//}
