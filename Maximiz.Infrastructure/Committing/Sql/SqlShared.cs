using Maximiz.Model.Entity;
using static Maximiz.Infrastructure.Querying.QueryExtractor;
using static Maximiz.Infrastructure.Querying.DatabaseColumnMap;
using System;

namespace Maximiz.Infrastructure.Committing.Sql
{

    /// <summary>
    /// Contains the SQL functions for generic queries.
    /// </summary>
    internal static partial class Sql
    {

        /// <summary>
        /// Generates an SQL statement for getting a single <see cref="Entity"/>
        /// with id equal to <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="id">Internal database id</param>
        /// <returns>SQL statement</returns>
        internal static string SqlGet<TEntity>(Guid id)
            where TEntity : Entity
            => $"SELECT * FROM {GetTableName<TEntity>()} WHERE id = '{id}';";

        /// <summary>
        /// Generates an SQL statement for deleting a single <see cref="Entity"/>
        /// with id equal to <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="id">Internal database id</param>
        /// <returns>SQL statement</returns>
        internal static string SqlDelete<TEntity>(Guid id)
            where TEntity : Entity
            => $"DELETE FROM {GetTableName<TEntity>()} WHERE id = '{id}';";

        /// <summary>
        /// Generates an SQl statement for checking if an <see cref="Entity"/>
        /// is available for claiming or modification. This will look at the
        /// <see cref="Model.Enums.OperationItemStatus"/> property.
        /// </summary>
        /// <remarks>
        /// Returns true only if the status is <see cref="Model.Enums.OperationItemStatus.UpToDate"/>.
        /// </remarks>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="id">Internal database id</param>
        /// <returns>SQL statement</returns>
        internal static string SqlIsAvailable<TEntity>(Guid id)
            where TEntity : Entity
            => $"SELECT CASE WHEN EXISTS (" +
            $" SELECT * FROM {GetTableName<TEntity>()}" +
            $" WHERE id = '{id}'" +
            $" AND operation_item_status = 'up_to_date')" + // TODO Hard coded?
            $" THEN CAST (1 AS BIT)" +
            $" ELSE CAST (0 AS BIT)" +
            $" END;";

        /// <summary>
        /// Generates an SQl statement for checking if an <see cref="Entity"/>
        /// is available and ready for deleting. This will look at the
        /// <see cref="Model.Enums.OperationItemStatus"/> property.
        /// </summary>
        /// <remarks>
        /// Returns true only if the status is <see cref="Model.Enums.OperationItemStatus.DeleteReady"/>.
        /// </remarks>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="id">Internal database id</param>
        /// <returns>SQL statement</returns>
        internal static string SqlIsDeleteReady<TEntity>(Guid id)
            where TEntity : Entity
            => $"SELECT CASE WHEN EXISTS (" +
            $" SELECT * FROM {GetTableName<TEntity>()}" +
            $" WHERE id = '{id}'" +
            $" AND operation_item_status = 'delete_ready')" + // TODO Hard coded?
            $" THEN CAST (1 AS BIT)" +
            $" ELSE CAST (0 AS BIT)" +
            $" END;";

    }
}
