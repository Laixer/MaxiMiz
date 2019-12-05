using Maximiz.Core.Operations;
using Maximiz.Core.StateMachine.Abstraction;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Maximiz.Core.StateMachine
{

    /// <summary>
    /// Manages our <see cref="IState"/> transitions.
    /// </summary>
    public sealed class StateManager : IStateManager
    {

        private IStateReader _stateReader;
        private ILogger logger;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public StateManager(IStateReader stateReader, ILoggerFactory loggerFactory)
        {
            _stateReader = stateReader ?? throw new ArgumentNullException(nameof(stateReader));

            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            logger = loggerFactory.CreateLogger(nameof(StateManager));
        }

        /// <summary>
        /// Attempts to start the state machine.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="Task"/></returns>
        public async Task AttemptStartStateMachineAsync(Operation operation)
        {
            // TODO Set some timer
            await AttemptContinueStateMachineAsync(operation);
        }

        /// <summary>
        /// Reads the current state and transitions if required. This will also
        /// attempt a transition until it succeeds or until the maximum amount 
        /// of attempts is exceeded. It will undo the transition if we can't make
        /// it.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="Task"/></returns>
        public async Task AttemptContinueStateMachineAsync(Operation operation)
        {
            var state = await _stateReader.GetCurrentStateAsync(operation);
            try
            {
                var transitionSuccess = await state.ExecuteTransitionAsync(operation);
                if (!transitionSuccess)
                {
                    var undoSuccess = await state.UndoTransitionAsync(operation);
                    if (!undoSuccess)
                    {
                        // If we cant undo our transition we should throw
                        logger.LogError($"Could not undo failed transition for {nameof(state)}");
                        throw new Exception(); // TODO How to do this elegant?
                    }
                }
                else if (state is IDontChainState)
                {
                    // Stop chaining events if this is explicitly set
                    logger.LogTrace($"Stopping chain state execution in state {nameof(state)}");
                    return;
                }
                else
                {
                    // Trigger next state machine event
                    await AttemptStartStateMachineAsync(operation);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Unexpected error while executing from state {nameof(state)} for operation {operation.Id}");
                // TODO What do we do here?
                // Maybe attempt to write to db, if that fails as well we cant do anything
            }
        }

    }

}
