using Dapper;
using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Infrastructure.Database;
using Maximiz.Model.Operations;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Maximiz.Infrastructure.Committing
{

    /// <summary>
    /// Committer for <see cref="MyOperation"/> items and all their corresponding
    /// actions. This committer is used to start a state machine operation.
    /// </summary>
    public sealed partial class OperationItemCommitter : IOperationItemCommitter
    {

        private readonly ILogger logger;
        private readonly IDatabaseProvider _databaseProvider;

        private readonly ICampaignCommitter _campaignCommitter;
        private readonly ICampaignGroupCommitter _campaignGroupCommitter;
        private readonly IAdItemCommitter _adItemCommitter;
        private readonly IAdGroupCommitter _adGroupCommitter;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public OperationItemCommitter(IDatabaseProvider databaseProvider,
            ILoggerFactory loggerFactory,
            ICampaignCommitter campaignCommitter,
            ICampaignGroupCommitter campaignGroupCommitter)
        {
            _databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            logger = loggerFactory.CreateLogger(nameof(OperationItemCommitter));
            _campaignCommitter = campaignCommitter ?? throw new ArgumentNullException(nameof(campaignCommitter));
            _campaignGroupCommitter = campaignGroupCommitter ?? throw new ArgumentNullException(nameof(campaignGroupCommitter));
        }

        /// <summary>
        /// Attempts to initialize an <see cref="MyOperation"/>. If we fail, this
        /// will throw an <see cref="InvalidOperationException"/>.
        /// TODO Is this the correct exception?
        /// </summary>
        /// <param name="operation"><see cref="MyOperation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        public async Task StartOperationOrThrowAsync(MyOperation operation, CancellationToken token)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }
            if (token == null) { throw new ArgumentNullException(nameof(token)); }

            using (var connection = _databaseProvider.GetConnectionScope())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        /* 
                         * TODO The issue with the connections.
                         * All possible approaches yielded errors, except the one
                         * where we reuse our connection object. Creating new
                         * connections gave "cannot read from stream" errors, to
                         * which I found no solutions online.
                         */
                        await ClaimOrThrowAsync(connection, operation, token);
                        await PrepareAllAsync(connection, operation, token);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        logger.LogError(e, "Inner exception while trying to start state machine operation");
                        throw new InvalidOperationException($"Could not start operation with id = {operation.Id}", e);
                    }
                }
            }
        }
    }

}
