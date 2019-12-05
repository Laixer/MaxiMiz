using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Core.StateMachine.Abstraction;
using Maximiz.Core.StateMachine.States;
using Microsoft.Extensions.Logging;

namespace Maximiz.Core.StateMachine
{
    public sealed class StateFactory : IStateFactory
    {

        private readonly IOperationCommitter _operationCommitter;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public StateFactory(IOperationCommitter operationCommitter,
            ILoggerFactory loggerFactory) 
        {
            _operationCommitter = operationCommitter;
            _loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Creates a new <see cref="DefaultState"/> state.
        /// </summary>
        /// <returns><see cref="DefaultState"/></returns>
        public DefaultState GetDefaultState()
            => new DefaultState(_operationCommitter, _loggerFactory, this);

        /// <summary>
        /// Creates a new <see cref="InitializationState"/> state.
        /// </summary>
        /// <returns><see cref="InitializationState"/></returns>
        public InitializationState GetInitializationState()
            => new InitializationState(_operationCommitter, _loggerFactory, this);

        /// <summary>
        /// Creates a new <see cref="DidNothingState"/> state.
        /// </summary>
        /// <returns><see cref="DidNothingState"/></returns>
        public DidNothingState GetDidNothingState()
            => new DidNothingState();

    }
}
