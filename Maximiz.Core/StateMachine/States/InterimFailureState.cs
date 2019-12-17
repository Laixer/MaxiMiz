using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Core.Infrastructure.EventQueue;
using Maximiz.Core.Infrastructure.Repositories;
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
    /// In this state our <see cref="Operation"/> execution has failed and we 
    /// need to populate our queue with the inverse of everything that has 
    /// already been (successfully) processed. When our queue is populated we
    /// will enter the <see cref="RollingBackState"/>.
    /// </summary>
    public sealed class InterimFailureState : IState
    {

        public int MaxExecuteAttempts => 25;
        public int MaxUndoAttempts => throw new InvalidOperationException($"State {nameof(InterimFailureState)} can't be undone");
        public TimeSpan DelayExecuteAttempt => TimeSpan.FromSeconds(90);
        public TimeSpan DelayExecuteUndo => throw new InvalidOperationException($"State {nameof(InterimFailureState)} can't be undone");

        private List<CreateOrUpdateObjectsMessage> messages = null;

        private readonly IOperationCommitter _operationCommitter;
        private readonly IOperationRepository _operationRepository;
        private readonly IOperationMessagesExtractor _operationMessagesExtractor;
        private readonly IEventQueueSender _eventQueueSender;
        private readonly ILogger logger;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public InterimFailureState(IOperationCommitter operationCommitter,
            IOperationRepository operationRepository, ILoggerFactory loggerFactory,
            IOperationMessagesExtractor operationMessagesExtractor, 
            IEventQueueSender eventQueueSender)
        {
            _operationCommitter = operationCommitter ?? throw new ArgumentNullException(nameof(operationCommitter));
            _operationRepository = operationRepository ?? throw new ArgumentNullException(nameof(operationRepository));
            _operationMessagesExtractor = operationMessagesExtractor ?? throw new ArgumentNullException(nameof(operationMessagesExtractor));
            _eventQueueSender = eventQueueSender ?? throw new ArgumentNullException(nameof(eventQueueSender));

            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            logger = loggerFactory.CreateLogger(nameof(InterimFailureState));
        }

        /// <summary>
        /// This will attempt to send a new <see cref="CreateOrUpdateObjectsMessage"/>
        /// to the queue in order to undo all changes that were made in the
        /// <see cref="ProcessingState"/>. 
        /// </summary>
        /// <remarks>
        /// This will create a new list of <see cref="CreateOrUpdateObjectsMessage"/>s
        /// the first time this function is called, or until this has succeeded.
        /// </remarks>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="true"/> if successful</returns>
        public async Task<bool> ExecuteTransitionAsync(Operation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            try
            {
                // Run this first time only, throws if it fails
                if (messages == null) { await SetupTransaction(operation); }

                // Send all messages that have not yet been sent
                var failedMessages = new List<CreateOrUpdateObjectsMessage>();
                foreach (var message in messages)
                {
                    if (!await _eventQueueSender.SendMessageAsync(message))
                    {
                        failedMessages.Add(message);
                    }
                }
                messages = failedMessages;

                // If no messages are left we are done
                return messages.Count == 0;
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error in transition for state {nameof(InterimFailureState)}");
                return false;
            }
        }

        /// <summary>
        /// Undoing queue sending is too complex, thus we simpmle throw an
        /// <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="InvalidOperationException"/></returns>
        public Task<bool> UndoTransitionAsync(Operation operation)
            => throw new InvalidOperationException($"Undo for state {nameof(InterimFailureState)} should never be reached");

        /// <summary>
        /// Marks all items in our <paramref name="operation"/> as rolling back
        /// and extract all messages. This function is atomic, and if it fails
        /// it will set <see cref="operationMessages"/> back to <see cref="null"/>.
        /// </summary>
        /// <remarks>
        /// This throws an <see cref="InvalidOperationException"/> in case of failure.
        /// </remarks>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="true"/> if successful</returns>
        private async Task SetupTransaction(Operation operation)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    messages = (await _operationMessagesExtractor.ExtractRollingBackMessagesAsync(operation)).ToList();
                    await _operationCommitter.MarkAllAsRollingBackAsync(operation);
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
