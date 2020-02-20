using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Repositories
{

    /// <summary>
    /// Contract for adding <see cref="OperationItemStatus"/> functionality to
    /// an <see cref="IRepository{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
    public interface IRepositoryStatus<TEntity>
        where TEntity : Entity
    {

        /// <summary>
        /// Shortcut to check if a <see cref="TEntity"/> has its status set
        /// to <see cref="OperationItemStatus.UpToDate"/>.
        /// </summary>
        /// <param name="id">Internal data store id</param>
        /// <returns><see cref="true"/> if available</returns>
        Task<bool> IsEntityAvailableAsync(Guid id);

        /// <summary>
        /// Gets the <see cref="OperationItemStatus"/> for a given <see cref="Entity"/>
        /// with data store id equal to <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Internal data store id</param>
        /// <returns><see cref="OperationItemStatus"/></returns>
        Task<OperationItemStatus> GetStatusAsync(Guid id);

        /// <summary>
        /// Sets the <see cref="OperationItemStatus"/> for a given <see cref="TEntity"/>
        /// with data store id equal to <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Internal data store id</param>
        /// <returns><see cref="Task"/></returns>
        Task SetStatusAsync(Guid id, OperationItemStatus status);

        /// <summary>
        /// Sets the <see cref="OperationItemStatus"/>es for a collection of
        /// <see cref="TEntity"/>s in our data store.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task SetStatusesAsync(IEnumerable<Guid> ids, OperationItemStatus status);

    }

}
