using Dapper;
using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Infrastructure.Database;
using Maximiz.Model.Entity;
using System;
using System.Threading;
using System.Threading.Tasks;

using static Maximiz.Infrastructure.Committing.Sql.Sql;
using static Maximiz.Infrastructure.Committing.CommitterSharedFunctions;
using System.Linq;
using Maximiz.Model.Enums;
using System.Data;

namespace Maximiz.Infrastructure.Committing
{

    /// <summary>
    /// Committer for <see cref="CampaignGroup"/>s.
    /// </summary>
    public sealed class CampaignGroupCommitter : ICampaignGroupCommitter
    {

        private readonly IDatabaseProvider _databaseProvider;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignGroupCommitter(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
        }

        /// <summary>
        /// Creates a <see cref="CampaignGroup"/> in our database.
        /// </summary>
        /// <param name="entity"><see cref="CampaignGroup"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>The created <see cref="CampaignGroup"/><returns>
        public async Task<CampaignGroup> CreateAsync(CampaignGroup entity, CancellationToken token)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            if (token == null) { throw new ArgumentNullException(nameof(token)); }
            //if (entity.Id != Guid.Empty && entity.Id != null) { throw new InvalidOperationException(nameof(entity)); } // TODO Fix!
            if (entity.OperationItemStatus != OperationItemStatus.PendingCreate) { throw new InvalidOperationException(nameof(entity.OperationItemStatus)); }

            using (var connection = _databaseProvider.GetConnectionScope())
            {
                try
                {
                    // TODO Do we really need to do this in this way?
                    var id = await connection.ExecuteScalarAsync<Guid>(new CommandDefinition(SqlCreateCampaignGroup(), entity, cancellationToken: token));
                    return (await connection.QueryAsync<CampaignGroup>(new CommandDefinition(SqlGet<CampaignGroup>(id), token))).First();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public Task<CampaignGroup> Delete(CampaignGroup entity, CancellationToken token) => throw new NotImplementedException();

        public Task<CampaignGroup> Update(CampaignGroup entity, CancellationToken token) => throw new NotImplementedException();

        public async Task<CampaignGroup> CreateAsyncFromConnection(IDbConnection connection, CampaignGroup entity, CancellationToken token)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            if (token == null) { throw new ArgumentNullException(nameof(token)); }
            //if (entity.Id != Guid.Empty && entity.Id != null) { throw new InvalidOperationException(nameof(entity)); } // TODO Fix!
            if (entity.OperationItemStatus != OperationItemStatus.PendingCreate) { throw new InvalidOperationException(nameof(entity.OperationItemStatus)); }

            //using (var connection = _databaseProvider.GetConnectionScope())
            var id = await connection.ExecuteScalarAsync<Guid>(new CommandDefinition(SqlCreateCampaignGroup(), entity, cancellationToken: token));
            entity.Id = id; // TODO Like this?
            return (await connection.QueryAsync<CampaignGroup>(new CommandDefinition(SqlGet<CampaignGroup>(id), token))).First();
        }

    }
}
