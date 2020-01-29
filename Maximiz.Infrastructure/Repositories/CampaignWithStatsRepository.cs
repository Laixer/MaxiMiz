using Dapper;
using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Core.Querying;
using Maximiz.Infrastructure.Database;
using Maximiz.Infrastructure.Querying;
using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{

    /// <summary>
    /// Repository for <see cref="CampaignWithStats"/>.
    /// </summary>
    public sealed class CampaignWithStatsRepository : RepositoryBase<CampaignWithStats>, ICampaignWithStatsRepository
    {

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignWithStatsRepository(IDatabaseProvider databaseProvider)
            : base(databaseProvider) { }

        /// <summary>
        /// Gets a single <see cref="CampaignWithStats"/> from our database based 
        /// on its internal id.
        /// </summary>
        /// <param name="id">Internal id</param>
        /// <returns><see cref="CampaignWithStats"/></returns>
        public override Task<CampaignWithStats> GetAsync(Guid id)
            => RepositorySharedFunctions.GetAsync<CampaignWithStats>(_databaseProvider, id, (x) => x.Id);

        public Task<IEnumerable<CampaignWithStats>> GetInactiveAsync(QueryBase<CampaignWithStats> query)
            => GetFromCampaignStatusesAsync(new List<CampaignStatus> {
                CampaignStatus.Depleted,
                CampaignStatus.DepletedMonthly,
                CampaignStatus.Expired,
                CampaignStatus.Frozen,
                CampaignStatus.Paused,
                CampaignStatus.Rejected,
                CampaignStatus.Terminated },
                query);

        public Task<IEnumerable<CampaignWithStats>> GetActiveAsync(QueryBase<CampaignWithStats> query)
            => GetFromCampaignStatusesAsync(new List<CampaignStatus> {
                CampaignStatus.Running },
                query);

        public Task<IEnumerable<CampaignWithStats>> GetPendingAsync(QueryBase<CampaignWithStats> query)
            => GetFromCampaignStatusesAsync(new List<CampaignStatus> {
                CampaignStatus.PendingApproval,
                CampaignStatus.PendingStartDate },
                query);

        /// <summary>
        /// Gets all <see cref="CampaignWithStats"/> based on some status collection
        /// and a <paramref name="query"/>.
        /// 
        /// TODO Is the query nullable?
        /// </summary>
        /// <remarks>
        /// The statuses get queried with OR statements.
        /// </remarks>
        /// <param name="statuses"><see cref="CampaignStatus"/></param>
        /// <param name="query"><see cref="QueryBase{TEntity}"/></param>
        /// <returns><see cref="IEnumerable{CampaignWithStats}"/></returns>
        private async Task<IEnumerable<CampaignWithStats>> GetFromCampaignStatusesAsync(List<CampaignStatus> statuses, QueryBase<CampaignWithStats> query)
        {
            if (statuses == null) { throw new ArgumentNullException(nameof(statuses)); }
            if (statuses.Count == 0) { throw new InvalidOperationException(nameof(statuses)); }

            using (var connection = _databaseProvider.GetConnectionScope())
            using (var source = new CancellationTokenSource())
            {
                query.PropertyEquality = new PropertyEqualityEnum<CampaignWithStats, CampaignStatus>((x) => x.Status, statuses, EnumTranslator.Translate);
                var sql = QueryExtractor.ExtractSql(query);
                var result = await connection.QueryAsync<CampaignWithStats>(new CommandDefinition(sql, source.Token));
                return result ?? throw new NullReferenceException();
            }
        }

        public Task<int> GetCountActiveAsync(QueryBase<CampaignWithStats> query)
            => GetCountByStatusAsync(new List<CampaignStatus> {
                CampaignStatus.Running },
                query);

        public Task<int> GetCountInactiveAsync(QueryBase<CampaignWithStats> query)
            => GetCountByStatusAsync(new List<CampaignStatus> {
                CampaignStatus.Depleted,
                CampaignStatus.DepletedMonthly,
                CampaignStatus.Expired,
                CampaignStatus.Frozen,
                CampaignStatus.Paused,
                CampaignStatus.Rejected,
                CampaignStatus.Terminated },
                query);

        public Task<int> GetCountPendingAsync(QueryBase<CampaignWithStats> query)
            => GetCountByStatusAsync(new List<CampaignStatus> {
                CampaignStatus.PendingApproval,
                CampaignStatus.PendingStartDate },
                query);

        private  Task<int> GetCountByStatusAsync(List<CampaignStatus> statuses, QueryBase<CampaignWithStats> query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            if (statuses == null) { throw new ArgumentNullException(nameof(statuses)); }
            if (statuses.Count == 0) { throw new InvalidOperationException(nameof(statuses)); }

            query.PropertyEquality = new PropertyEqualityEnum<CampaignWithStats, CampaignStatus>((x) => x.Status, statuses, EnumTranslator.Translate);
            return GetCountAsync(query);
        }

    }
}
