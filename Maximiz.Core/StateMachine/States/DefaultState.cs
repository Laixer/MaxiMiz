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
        /// <summary>
        /// Number of times we may attempt to do our transition.
        /// </summary>
        private const int maxTransitionAttempts = 25;

        /// <summary>
        /// The time we should wait before retrying our transition after failure.
        /// </summary>
        private readonly TimeSpan transitionRetryInterval = new TimeSpan(hours: 0, minutes: 0, seconds: 30);

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

            for (int i = 0; i < maxTransitionAttempts; i++)
            {
                try
                {
                    await _operationCommitter.MarkAllAsPending(operation);
                    logger.LogTrace($"Successfully made transition for {nameof(DefaultState)}");
                    return true;
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message, $"Error in transition attempt {i+1} for state {nameof(DefaultState)}");
                }

                // Wait if we didn't make it the first time
                await Task.Delay(transitionRetryInterval);
            }

            // If we reach this point we have failed doing our transition
            logger.LogError($"Could not perform transition for {nameof(DefaultState)} after {maxTransitionAttempts} attempts");
            return false;
        }

        /// <summary>
        /// Does nothing, because database transactions are atomic.
        /// </summary>
        /// <remarks>
        /// Just returns <see cref="true"/>.
        /// </remarks>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="true"/></returns>
        public Task<bool> UndoTransitionAsync(Operation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            return Task.FromResult(true);
        }

    }

}
