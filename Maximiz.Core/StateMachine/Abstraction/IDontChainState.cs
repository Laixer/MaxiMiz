
namespace Maximiz.Core.StateMachine.Abstraction
{

    /// <summary>
    /// Indicates that the executor of this <see cref="IState"/> should stop
    /// executing after this has completed. Some external event will trigger
    /// the next state machine transition.
    /// </summary>
    public interface IDontChainState : IState { }

}
