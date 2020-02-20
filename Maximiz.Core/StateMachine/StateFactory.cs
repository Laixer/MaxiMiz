using Maximiz.Core.StateMachine.Abstraction;
using System;

namespace Maximiz.Core.StateMachine
{

    /// <summary>
    /// Factory for our <see cref="IState"/>s.
    /// TODO Implement this with <see cref="https://rogerjohansson.blog/2008/02/28/linq-expressions-creating-objects/"/>.
    /// </summary>
    public sealed class StateFactory : IStateFactory
    {

        /// <summary>
        /// Creates an instance of <typeparamref name="TState"/>.
        /// TODO This is slow, implement differently.
        /// </summary>
        /// <typeparam name="TState"><see cref="IState"/></typeparam>
        /// <returns><see cref="IState"/></returns>
        public TState GetState<TState>()
            where TState : class, IState
            => (TState)Activator.CreateInstance(typeof(TState));

    }
}
