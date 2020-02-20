using Maximiz.Model.Operations;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Commiting
{

    /// <summary>
    /// Contract for managing item status during a state machine operation.
    /// </summary>
    public interface IOperationItemCommitter
    {

        Task StartOperationOrThrowAsync(MyOperation operation, CancellationToken token);

    }
}
