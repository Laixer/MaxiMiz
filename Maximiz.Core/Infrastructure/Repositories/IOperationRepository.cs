using Maximiz.Core.Operations;
using System;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Repositories
{

    /// <summary>
    /// Contract for an <see cref="Operation"/> repository.
    /// </summary>
    public interface IOperationRepository : IRepository<Operation>
    {

        /// <summary>
        /// Checks to see if a given <see cref="Operation"/> is locked.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <returns><see cref="true"/> if the <see cref="Operation"/> is locked</returns>
        Task<bool> IsOperationLockedAsync(Operation operation);

        /// <summary>
        /// Check to see if a given <see cref="Operation"/> is locked by a given
        /// lock id: <paramref name="lockId"/>.
        /// </summary>
        /// <param name="operation"><see cref="Operation"/></param>
        /// <param name="lockId">The lock <see cref="Guid"/></param>
        /// <returns><see cref="true"/> if the <paramref name="lockId"/> matches
        /// the actual lock id in the data store</returns>
        Task<bool> IsOperationLockedByIdAsync(Operation operation, Guid lockId);

    }
}
