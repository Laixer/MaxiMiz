using Dapper;
using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Infrastructure.Committing.Sql;
using Maximiz.Infrastructure.Database;
using Maximiz.Model.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Committing
{

    /// <summary>
    /// Commits <see cref="Campaign"/>s to our internal data store.
    /// Implements <see cref="ICampaignCommitter"/>.
    /// </summary>
    public sealed class CampaignCommitter : ICampaignCommitter
    {

        private readonly IDatabaseProvider _databaseProvider;
        private readonly ILogger logger;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignCommitter(IDatabaseProvider databaseProvider, ILoggerFactory loggerFactory)
        {
            if (databaseProvider == null) { throw new ArgumentNullException(nameof(databaseProvider)); }
            _databaseProvider = databaseProvider;

            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
            logger = loggerFactory.CreateLogger(nameof(CampaignCommitter));
        }

        /// <summary>
        /// Creates a given <see cref="Campaign"/> in our data store and returns
        /// the result.
        /// </summary>
        /// <param name="entity"><see cref="Campaign"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Campaign"/></returns>
        public async Task<Campaign> Create(Campaign entity, CancellationToken token)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            if (token == null) { throw new ArgumentNullException(nameof(token)); }

            using (var connection = _databaseProvider.GetConnectionScope())
            {
                var id = await connection.ExecuteScalarAsync<Guid>(new CommandDefinition(
                    SqlCampaignCommitter.CreateCampaign, entity, cancellationToken: token));
                var created = (await connection.QueryAsync<Campaign>(new CommandDefinition(
                    SqlCampaignCommitter.GetCampaign, id, cancellationToken: token))).FirstOrDefault();

                // TODO Specify exception?
                if (created == null) { throw new Exception($"Something went wrong while creating a new campaign in our database. Id = {id}"); }
                logger.LogTrace("Created campaign successfully");
                return created;
            }
        }

        /// <summary>
        /// Deletes a <see cref="Campaign"/> in our data store.
        /// </summary>
        /// <remarks>
        /// This will simply return its input parameter <see cref="Campaign"/>.
        /// </remarks>
        /// <param name="entity"><see cref="Campaign"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>Input parameter <see cref="Campaign"/></returns>
        public async Task<Campaign> Delete(Campaign entity, CancellationToken token)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            if (token == null) { throw new ArgumentNullException(nameof(token)); }

            using (var connection = _databaseProvider.GetConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    SqlCampaignCommitter.DeleteCampaign, entity, cancellationToken: token));
                logger.LogTrace("Deleted campaign successfully");
                return entity;
            }
        }

        /// <summary>
        /// Updates a <see cref="Campaign"/> in our data store.
        /// </summary>
        /// <remarks>
        /// This will simply return its input parameter <see cref="Campaign"/>.
        /// </remarks>
        /// <param name="entity"><see cref="Campaign"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>Input parameter <see cref="Campaign"/></returns>
        public async Task<Campaign> Update(Campaign entity, CancellationToken token)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            if (token == null) { throw new ArgumentNullException(nameof(token)); }

            using (var connection = _databaseProvider.GetConnectionScope())
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    SqlCampaignCommitter.UpdateCampaign, entity, cancellationToken: token));
                logger.LogTrace("Updated campaign successfully");
                return entity;
            }
        }

        /// <summary>
        /// TODO Implement.
        /// </summary>
        public Task<bool> UpdateBulkAsync(IEnumerable<Campaign> entities, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
