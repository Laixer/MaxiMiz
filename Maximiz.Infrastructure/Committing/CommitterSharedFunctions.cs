using Dapper;
using Maximiz.Infrastructure.Database;
using Maximiz.Model.Entity;
using System;
using System.Data;
using System.Threading.Tasks;
using static Maximiz.Infrastructure.Committing.Sql.Sql;

namespace Maximiz.Infrastructure.Committing
{

    /// <summary>
    /// Shared functions between <see cref="Core.Infrastructure.Commiting.ICommitter{TEntity}"/> interfaces.
    /// </summary>
    internal static class CommitterSharedFunctions
    {

        /// <summary>
        /// Validates if an <see cref="Entity"/> is available in the database, 
        /// meaning it has its <see cref="Model.Enums.OperationItemStatus"/>
        /// set to <see cref="Model.Enums.OperationItemStatus.UpToDate"/>. If 
        /// this is not the case this will throw an <see cref="EntityInOperationException"/>.
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="connection"><see cref="IDbConnection"/></param>
        /// <param name="id">Internal database id</param>
        /// <returns><see cref="Task"/></returns>
        internal static async Task ThrowIfUnavailable<TEntity>(IDbConnection connection, Guid id)
            where TEntity : Entity
        {
            if (connection == null) { throw new ArgumentNullException(nameof(connection)); }
            if (id == null || id == Guid.Empty) { throw new ArgumentNullException(nameof(id)); }

            if (!await connection.ExecuteScalarAsync<bool>(new CommandDefinition(SqlIsAvailable<Campaign>(id))))
            {
                throw new EntityInOperationException($"Entity {id} is not available for modification or claiming");
            }
        }

        /// <summary>
        /// Validates if an <see cref="Entity"/> is delete ready in the database, 
        /// meaning it has its <see cref="Model.Enums.OperationItemStatus"/>
        /// set to <see cref="Model.Enums.OperationItemStatus.DeleteReady"/>. If 
        /// this is not the case this will throw an <see cref="EntityInOperationException"/>.
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="connection"><see cref="IDbConnection"/></param>
        /// <param name="id">Internal database id</param>
        /// <returns><see cref="Task"/></returns>
        internal static async Task ThrowIfNotDeleteReady<TEntity>(IDbConnection connection, Guid id)
            where TEntity : Entity
        {
            if (connection == null) { throw new ArgumentNullException(nameof(connection)); }
            if (id == null || id == Guid.Empty) { throw new ArgumentNullException(nameof(id)); }

            if (!await connection.ExecuteScalarAsync<bool>(new CommandDefinition(SqlIsDeleteReady<Campaign>(id))))
            {
                throw new EntityInOperationException($"Entity {id} is not available to be deleted");
            }
        }

    }
}
