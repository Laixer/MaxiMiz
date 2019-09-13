using System.Threading;
using System.Threading.Tasks;

namespace Poller.Poller
{

    /// <summary>
    /// Used to perform CRUD operations on entities created. This is generic
    /// for all providers and uses the <see cref="CreateOrUpdateObjectsContext"/>
    /// to indicate what should happen.
    /// </summary>
    public interface IPollerCreateOrUpdateObjects : IPoller
    {

        /// <summary>
        /// A CRUD call, based on a context containing the specific details of
        /// what has to be executed.
        /// </summary>
        /// <param name="contex">The CRUD context</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        Task CreateOrUpdateObjectsAsync(CreateOrUpdateObjectsContext contex, CancellationToken token);
    }
}
