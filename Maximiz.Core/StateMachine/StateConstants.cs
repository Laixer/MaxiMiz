
namespace Maximiz.Core.StateMachine
{

    /// <summary>
    /// Contains all our global constants for the state machine.
    /// </summary>
    public static class StateConstants
    {

        /// <summary>
        /// The maximum amount of times we can enter the failure state.
        /// </summary>
        public static int MaxFailureCount { get; } = 5;

    }
}
