using Dapper;
using Maximiz.Core.Querying;
using Maximiz.Infrastructure.Database;
using Maximiz.Infrastructure.Querying;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{

    /// <summary>
    /// Contains all shared functions across <see cref="IRepository"/> implementations.
    /// </summary>
    internal static class RepositorySharedFunctions
    {

        internal static async Task<TEntity> GetAsync<TEntity>(IDatabaseProvider databaseProvider, Guid id, Expression<Func<TEntity, object>> idProperty)
            where TEntity : Entity
        {
            if (id == null || id == Guid.Empty) { throw new ArgumentNullException(nameof(id)); }

            using (var connection = databaseProvider.GetConnectionScope())
            using (var source = new CancellationTokenSource())
            {
                var sql = QueryExtractor.ExtractSqlForSingle(new PropertyEquality<TEntity>(idProperty, id));
                var result = await connection.QueryAsync<TEntity>(new CommandDefinition(sql, cancellationToken: source.Token));
                if (result == null) { throw new ArgumentNullException(nameof(result)); }
                if (result.AsList().Count == 0) { throw new ArgumentNullException(nameof(result)); }
                return result.AsList()[0];
            }
        }

        internal static async Task<TEntity> GetFromExternalIdAsync<TEntity>(IDatabaseProvider databaseProvider, string externalId, Expression<Func<TEntity, object>> externalIdProperty)
            where TEntity : Entity
        {
            if (string.IsNullOrEmpty(externalId)) { throw new ArgumentNullException(nameof(externalId)); }

            using (var connection = databaseProvider.GetConnectionScope())
            using (var source = new CancellationTokenSource())
            {
                var sql = QueryExtractor.ExtractSqlForSingle(new PropertyEquality<TEntity>(externalIdProperty, externalId));
                var result = await connection.QueryAsync<TEntity>(new CommandDefinition(sql, cancellationToken: source.Token));
                if (result == null) { throw new ArgumentNullException(nameof(result)); }
                if (result.AsList().Count == 0) { throw new ArgumentNullException(nameof(result)); }
                return result.AsList()[0];
            }
        }

        /// <summary>
        /// Queries multiple <typeparamref name="TEntity"/>s from our database.
        /// 
        /// TODO Is this safe? We can just put any SQL in here (I do think so since its internal).
        /// 
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="databaseProvider"><see cref="IDatabaseProvider"/></param>
        /// <param name="sql">SQL string</param>
        /// <returns><see cref="IEnumerable{TEntity}"/></returns>
        internal static async Task<IEnumerable<TEntity>> QueryAsync<TEntity>(IDatabaseProvider databaseProvider, string sql)
            where TEntity : Entity
        {
            if (string.IsNullOrEmpty(sql)) { throw new ArgumentNullException(nameof(sql)); }
            // TODO More validity checks? Not necessary I think.

            using (var connection = databaseProvider.GetConnectionScope())
            using (var source = new CancellationTokenSource())
            {
                var result = await connection.QueryAsync<TEntity>(new CommandDefinition(sql, source.Token));
                return result ?? throw new NullReferenceException();
            }
        }

    }
}
