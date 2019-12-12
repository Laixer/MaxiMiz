namespace Maximiz.Core.StateMachine.Abstraction
{

    /// <summary>
    /// Contract for creating new <see cref="IState"/>s.
    /// </summary>
    public interface IStateFactory
    {

        /// <summary>
        /// Creates an <see cref="IState"/> of type <typeparamref name="TState"/>.
        /// </summary>
        /// <typeparam name="TState"><see cref="IState"/></typeparam>
        /// <returns><see cref="IState"/></returns>
        TState GetState<TState>() where TState : class, IState;

    }
}
