using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Core.Infrastructure.EventQueue;
using Maximiz.Core.Operations;
using Maximiz.Core.Operations.Execution;
using Maximiz.Core.StateMachine.Abstraction;
using Maximiz.Model.Protocol;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Core.StateMachine.States
{

    /// <summary>
    /// State in which all our respective operation items are marked as pending
    /// in the data store.
    /// </summary>
    public sealed class MarkedPendingState : IState
    {

        /// <summary>
        /// Number of times we may attempt to do our transition.
        /// </summary>
        private const int maxTransitionAttempts = 25;

        /// <summary>
        /// The time we should wait before retrying our transition after failure.
        /// </summary>
        private readonly TimeSpan transitionRetryInterval = new TimeSpan(hours: 0, minutes: 0, seconds: 30);

        private IOperationCommitter _operationCommitter;
        private IEventQueueSender _eventQueueSender;
        private IOperationMessagesExtractor _operationMessagesExtractor;
        private ILogger logger;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public MarkedPendingState(IOperationCommitter operationCommitter, ILoggerFactory loggerFactory,
            IEventQueueSender eventQueueSender, IOperationMessagesExtractor operationMessagesExtractor)
        {
            _operationCommitter = operationCommitter ?? throw new ArgumentNullException(nameof(operationCommitter));
            _eventQueueSender = eventQueueSender ?? throw new ArgumentNullException(nameof(eventQueueSender));
            _operationMessagesExtractor = operationMessagesExtractor ?? throw new ArgumentNullException(nameof(operationMessagesExtractor));
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            logger = loggerFactory.CreateLogger(nameof(DefaultState));
        }

        /// <summary>
        /// Attempts to send all items in the <paramref name="operation"/> to
        /// the queue for processing.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="true"/> if successful</returns>
        public async Task<bool> ExecuteTransitionAsync(Operation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            var messages = _operationMessagesExtractor.Extract(operation);
            for (int i = 0; i < maxTransitionAttempts; i++)
            {
                try
                {
                    // Send all messages, save failed for next round
                    await _operationCommitter.MarkAllAsProcessing(operation);
                    var messagesFailed = new List<CreateOrUpdateObjectsMessage>();
                    foreach (var message in messages)
                    {
                        if (!await _eventQueueSender.SendMessageAsync(message))
                        {
                            messagesFailed.Add(message);
                        }
                    }
                    if (messagesFailed.Count == 0) { return true; }
                    else { messages = messagesFailed; }
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message, $"Error in transition attempt {i + 1} for state {nameof(MarkedPendingState)}");
                }

                // Wait if we didn't make it the first time
                await Task.Delay(transitionRetryInterval);
            }

            // If we reach this point we have failed doing our transition
            logger.LogError($"Could not perform transition for {nameof(MarkedPendingState)} after {maxTransitionAttempts} attempts");
            return false;
        }


        /// <summary>
        /// We can't undo our enqueueing, this introduces too much complexity.
        /// The undo will therefor always fail.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="false"/></returns>
        public Task<bool> UndoTransitionAsync(Operation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }
            return Task.FromResult(false);
        }
    }

}
