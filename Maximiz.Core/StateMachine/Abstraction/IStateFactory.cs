using Maximiz.Core.StateMachine.States;

namespace Maximiz.Core.StateMachine.Abstraction
{

    /// <summary>
    /// Contract for creating new <see cref="IState"/>s.
    /// </summary>
    public interface IStateFactory
    {

        /// <summary>
        /// Creates a new <see cref="DefaultState"/> state.
        /// </summary>
        /// <returns><see cref="DefaultState"/></returns>
        DefaultState GetDefaultState();

        /// <summary>
        /// Creates a new <see cref="MarkedPendingState"/> state.
        /// </summary>
        /// <returns><see cref="MarkedPendingState "/></returns>
        MarkedPendingState GetMarkedPendingState();

    }
}
