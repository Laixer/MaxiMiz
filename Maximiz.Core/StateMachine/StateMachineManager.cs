using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Core.Infrastructure.EventQueue;
using Maximiz.Core.StateMachine.Abstraction;
using Maximiz.Model.Operations;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Core.StateMachine
{

    /// <summary>
    /// This is used to start and continue our state machine.
    /// </summary>
    public sealed class StateMachineManager : IStateMachineManager
    {

        private readonly ILogger logger;
        private readonly IOperationItemCommitter _operationItemCommitter;
        private readonly IEventQueueSender _eventQueueSender;

        /// <summary>
        /// Constructor for dependency injection,
        /// </summary>
        public StateMachineManager(ILoggerFactory loggerFactory,
            IOperationItemCommitter operationItemCommitter,
            IEventQueueSender eventQueueSender)
        {
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            logger = loggerFactory.CreateLogger(nameof(StateMachineManager));
            _operationItemCommitter = operationItemCommitter ?? throw new ArgumentNullException(nameof(operationItemCommitter));
            _eventQueueSender = eventQueueSender ?? throw new ArgumentNullException(nameof(eventQueueSender));
        }

        /// <summary>
        /// Attempts to launch a <see cref="MyOperation"/>, this will throw if
        /// we fail.
        /// </summary>
        /// <param name="operation"><see cref="MyOperation"/></param>
        /// <returns><see cref="Task"/></returns>
        public async Task AttemptStartStateMachineAsync(MyOperation operation, CancellationToken token)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }
            if (operation.Id == null || operation.Id == Guid.Empty) { throw new ArgumentNullException(nameof(operation.Id)); }
            if (token == null) { throw new ArgumentNullException(nameof(token)); }
            if (token.IsCancellationRequested) { throw new OperationCanceledException(); }

            try
            {
                await _operationItemCommitter.StartOperationOrThrowAsync(operation, token);
                throw new NotImplementedException();
                await _eventQueueSender.SendMessageAsync(null);
            }
            catch (Exception e)
            {
                logger.LogError($"Error while starting state machine for operation with id = {operation.Id}", e);
                throw new InvalidOperationException("Could not start state machine", e);
            }
        }
    }
}
