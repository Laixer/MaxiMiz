using Maximiz.Core.Operations;
using Maximiz.Model.Operations;
using System.Threading.Tasks;

namespace Maximiz.Core.StateMachine.Abstraction
{

    /// <summary>
    /// Contract for reading the current state of an operation.
    /// </summary>
    public interface IStateReader
    {

        /// <summary>
        /// Reads the current state based on all internal and external variables.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="IState"/></returns>
        Task<IState> GetCurrentStateAsync(Operation operation);

    }
}
