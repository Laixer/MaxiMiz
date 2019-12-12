
namespace Maximiz.Core.StateMachine.Abstraction
{

    /// <summary>
    /// Contract indicating our <see cref="IState"/> is passive and contains
    /// no functionality for state transitioning.
    /// </summary>
    public interface IPassiveState : IState { }

}
