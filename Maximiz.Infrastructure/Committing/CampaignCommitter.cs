using Dapper;
using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Infrastructure.Database;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using static Maximiz.Infrastructure.Committing.Sql.Sql;
using static Maximiz.Infrastructure.Committing.CommitterSharedFunctions;
using System.Data;

namespace Maximiz.Infrastructure.Committing
{

    /// <summary>
    /// Committer for <see cref="Campaign"/> entities.
    /// </summary>
    public sealed class CampaignCommitter : ICampaignCommitter
    {

        private readonly IDatabaseProvider _databaseProvider;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignCommitter(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
        }

        /// <summary>
        /// Creates a <see cref="Campaign"/> in our database and returns it,
        /// including its id which was created by our database.
        /// </summary>
        /// <param name="entity"><see cref="Campaign"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Campaign"/></returns>
        public async Task<Campaign> CreateAsync(Campaign entity, CancellationToken token)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            if (token == null) { throw new ArgumentNullException(nameof(token)); }
            // TODO Model validation check?

            using (var connection = _databaseProvider.GetConnectionScope())
            {
                var id = await connection.ExecuteScalarAsync<Guid>(new CommandDefinition(SqlCreateCampaign(), entity, cancellationToken: token));
                return (await connection.QueryAsync<Campaign>(new CommandDefinition(SqlGet<Campaign>(id), token))).First();
            }
        }

        /// <summary>
        /// Deletes a <see cref="Campaign"/> from our database and returns it.
        /// </summary>
        /// <remarks>
        /// This just returns <paramref name="entity"/>.
        /// </remarks>
        /// <param name="entity"><see cref="Campaign"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        public async Task<Campaign> Delete(Campaign entity, CancellationToken token)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            if (token == null) { throw new ArgumentNullException(nameof(token)); }

            using (var connection = _databaseProvider.GetConnectionScope())
            {
                await ThrowIfNotDeleteReady<Campaign>(connection, entity.Id); // Check if we are allowed to delete
                await connection.ExecuteAsync(new CommandDefinition(SqlDelete<Campaign>(entity.Id), token));
                return entity; // TODO Do we really need to do this?
            }
        }

        /// <summary>
        /// Updates a <see cref="Campaign"/> in our database and returns it.
        /// </summary>
        /// <remarks>
        /// This just returns <paramref name="entity"/>.
        /// </remarks>
        /// <param name="entity"><see cref="Campaign"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Campaign"/></returns>
        public async Task<Campaign> Update(Campaign entity, CancellationToken token)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            if (token == null) { throw new ArgumentNullException(nameof(token)); }
            // TODO Model validation check?

            using (var connection = _databaseProvider.GetConnectionScope())
            {
                await ThrowIfUnavailable<Campaign>(connection, entity.Id); // Check if we are allowed to modify.
                await connection.ExecuteAsync(new CommandDefinition(SqlUpdateCampaign(), entity, cancellationToken: token));
                return entity;
                // TODO Do we really need to do this?
                // TODO Maybe re-query it from the db?
            }
        }

        /// <summary>
        /// Creates a collections of <see cref="Campaign"/>s in a single statement.
        /// </summary>
        /// <param name="campaigns"><see cref="Campaign"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>The created <see cref="Campaign"/>s</returns>
        public async Task CreateBulkAsync(IEnumerable<Campaign> campaigns, CancellationToken token, IDbConnection connection)
        {
            if (campaigns == null) { throw new ArgumentNullException(nameof(campaigns)); }
            if (token == null) { throw new ArgumentNullException(nameof(token)); }
            if (connection == null) { throw new ArgumentNullException(nameof(connection)); }
            if (connection.State != ConnectionState.Open) { throw new InvalidOperationException("DB connection must be opened"); }

            try
            {
                await connection.ExecuteAsync(new CommandDefinition(SqlCreateCampaign(), campaigns, cancellationToken: token));
            } catch (Exception e)
            {
                throw e;
            }
        }

        public Task<bool> UpdateBulkAsync(IEnumerable<Campaign> entities, CancellationToken token)
        {
            throw new NotImplementedException();
        }

    }
}
