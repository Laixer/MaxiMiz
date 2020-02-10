using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Core.Infrastructure.EventQueue;
using Maximiz.Core.Operations;
using Maximiz.Core.Operations.Execution;
using Maximiz.Core.StateMachine.Abstraction;
using Maximiz.Model.Operations;
using Maximiz.Model.Protocol;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Maximiz.Core.StateMachine.States
{

    /// <summary>
    /// State in which all our respective operation items are marked as pending
    /// in the data store.
    /// </summary>
    public sealed class MarkedPendingState : IState
    {

        public int MaxExecuteAttempts => 25;
        public int MaxUndoAttempts => 25;
        public TimeSpan DelayExecuteAttempt => TimeSpan.FromSeconds(90);
        public TimeSpan DelayExecuteUndo => TimeSpan.FromSeconds(90);

        private readonly IOperationCommitter _operationCommitter;
        private readonly IEventQueueSender _eventQueueSender;
        private readonly IOperationMessagesExtractor _operationMessagesExtractor;
        private readonly ILogger logger;

        /// <summary>
        /// Used to store all queue messages that have to be sent. This is null
        /// at the time this state is created, to allow us to keep track of the
        /// messages sent in previous attempts.
        /// </summary>
        private List<CreateOrUpdateObjectsMessage> messages = null;

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
        /// TODO DRY with <see cref="InterimFailureState"/>.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="true"/> if successful</returns>
        public async Task<bool> ExecuteTransitionAsync(Operation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            try
            {
                // Run this first time only, throws if it fails
                if (messages == null) { await SetupTransition(operation); }

                throw new NotImplementedException();
                //// Send all messages that are left
                //var failedMessages = new List<CreateOrUpdateObjectsMessage>();
                //foreach (var message in messages)
                //{
                //    if (!await _eventQueueSender.SendMessageAsync(message))
                //    {
                //        failedMessages.Add(message);
                //    }
                //}
                //messages = failedMessages;

                // If no messages are left we are done
                return messages.Count == 0;
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error in transition for state {nameof(MarkedPendingState)}");
                return false;
            }
        }


        /// <summary>
        /// We can't undo our enqueueing, this introduces too much complexity.
        /// The undo will therefor always fail.
        /// TODO Shouldn't this just throw an <see cref="InvalidOperationException"/>?
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="false"/></returns>
        public Task<bool> UndoTransitionAsync(Operation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            return Task.FromResult(false);
        }

        /// <summary>
        /// Marks all items in our <paramref name="operation"/> as processing
        /// and extract all messages. This function is atomic, and if it fails
        /// it will set <see cref="messages"/> back to <see cref="null"/>.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="true"/> if successful</returns>
        private async Task SetupTransition(Operation operation)
        {

            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _operationCommitter.MarkAllAsProcessingAsync(operation);
                    messages = (await _operationMessagesExtractor.ExtractMessages(operation)).ToList();
                    transactionScope.Complete();
                    return;
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                    messages = null;
                    throw new InvalidOperationException($"Error while creating and setting messages for operation { operation.Id}");
                }
            }
        }

    }

}
