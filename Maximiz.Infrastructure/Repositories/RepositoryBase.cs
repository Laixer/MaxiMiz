using Dapper;
using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Core.Querying;
using Maximiz.Infrastructure.Database;
using Maximiz.Infrastructure.Querying;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{

    /// <summary>
    /// Contains the overlapping functionality for each repository which can 
    /// be declared 100% generic.
    /// </summary>
    /// <remarks>
    /// Anything that uses some kind of property expression can't be put in here.
    /// </remarks>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {

        protected readonly IDatabaseProvider _databaseProvider;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        protected RepositoryBase(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
        }

        /// <summary>
        /// Gets all <see cref="TEntity"/>s from the database based on only the page.
        /// </summary>
        /// <param name="page">Page number</param>
        /// <returns><see cref="IEnumerable{TEntity}"/></returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync(int page = 0)
        {
            if (page < 0) { throw new ArgumentOutOfRangeException(nameof(page)); }

            using (var connection = _databaseProvider.GetConnectionScope())
            using (var source = new CancellationTokenSource())
            {
                var sql = QueryExtractor.ExtractSql<TEntity>(page);
                var result = await connection.QueryAsync<TEntity>(new CommandDefinition(sql, cancellationToken: source.Token));
                return result ?? throw new NullReferenceException();
            }
        }

        /// <summary>
        /// Returns all <see cref="TEntity"/>s based on some <paramref name="query"/>.
        /// </summary>
        /// <param name="query"><see cref="QueryBase{TEntity}"/></param>
        /// <returns><see cref="IEnumerable{TEntity}"/></returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync(QueryBase<TEntity> query)
        {
            if (query == null) { return await GetAllAsync(); }

            using (var connection = _databaseProvider.GetConnectionScope())
            using (var source = new CancellationTokenSource())
            {
                var sql = QueryExtractor.ExtractSql(query);
                var result = await connection.QueryAsync<TEntity>(new CommandDefinition(sql, source.Token));
                return result ?? throw new NullReferenceException();
            }
        }

        /// <summary>
        /// Gets the amount of <see cref="TEntity"/>s in the database based on
        /// some <paramref name="query"/>.
        /// </summary>
        /// <param name="query"><see cref="QueryBase{TEntity}"/></param>
        /// <returns>Count</returns>
        public async Task<int> GetCountAsync(QueryBase<TEntity> query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            using (var connection = _databaseProvider.GetConnectionScope())
            using (var source = new CancellationTokenSource())
            {
                var sql = QueryExtractor.ExtractSql(query, true);
                return await connection.ExecuteScalarAsync<int>(new CommandDefinition(sql, cancellationToken: source.Token));
            }
        }

        /// <summary>
        ///  This can't be done 100% generic.
        /// </summary>
        /// <param name="id">Internal id</param>
        /// <returns><see cref="TEntity"/></returns>
        public abstract Task<TEntity> GetAsync(Guid id);

    }
}
