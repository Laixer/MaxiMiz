using Dapper;
using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Infrastructure.Database;
using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using Maximiz.Model.Operations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

using static Maximiz.Infrastructure.Committing.Sql.Sql;
using static Maximiz.Infrastructure.Querying.QueryExtractor;
using static Maximiz.Infrastructure.Querying.DatabaseColumnMap;
using static Maximiz.Infrastructure.Querying.PropertyUtility;
using Microsoft.Extensions.Logging;
using Maximiz.Model;

namespace Maximiz.Infrastructure.Committing
{

    /// <summary>
    /// Handles all our database management for the <see cref="MyOperation"/> functionality.
    /// </summary>
    public sealed partial class OperationItemCommitter : IOperationItemCommitter
    {

        /// <summary>
        /// Claims all items for a given <see cref="Operation"/> or throws an
        /// <see cref="EntityInOperationException"/>.
        /// </summary>
        /// <remarks>
        /// If this does not throw, that means that the claiming is successful.
        /// </remarks>
        /// <param name="operation"><see cref="MyOperation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        private async Task ClaimOrThrowAsync(IDbConnection connection, MyOperation operation, CancellationToken token)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }
            if (operation.Id == null || operation.Id == Guid.Empty) { throw new ArgumentNullException(nameof(operation.Id)); }
            if (token == null) { throw new ArgumentNullException(nameof(token)); }

            try
            {
                await connection.QueryAsync(GetClaimSql(operation), token);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error while trying to claim entity with id = {operation.TopEntity.Id}");
                throw new EntityInOperationException($"Unable to claim entity with id = {operation.TopEntity.Id}");
            }
        }

        /// <summary>
        /// Gets the correct sql for claiming an <see cref="Entity"/> for an
        /// <see cref="MyOperation"/> in the database.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/></param>
        /// <returns>SQL statement</returns>
        private string GetClaimSql(MyOperation operation)
        {
            if (operation.Id == null || operation.Id == Guid.Empty) { throw new ArgumentNullException(nameof(operation.Id)); }
            if (operation.TopEntity == null) { throw new ArgumentNullException(nameof(operation.TopEntity)); }

            switch (operation.TopEntity)
            {
                // TODO that 2 is really ugly, maybe inline after all?
                case Campaign x:
                    var adGroupIds = operation.AdGroupCampaignLinksAdd.Select(y => y.AdGroupId).ToList();
                    return $"SELECT claim_campaign('{x.Id}', '{operation.Id}', {ComposeIdArray(adGroupIds)});";
                case CampaignGroup x:
                    var adGroupIds2 = operation.AdGroupCampaignGroupLinksAdd.Select(y => y.AdGroupId).ToList();
                    return $"SELECT claim_campaign_group('{x.Id}', '{operation.Id}', {ComposeIdArray(adGroupIds2)});";
                case AdItem x:
                    return $"SELECT claim_ad_item('{x.Id}', '{operation.Id}');";
                case AdGroup x:
                    return $"SELECT claim_ad_group('{x.Id}', '{operation.Id}');";
            }

            throw new InvalidOperationException(nameof(operation.TopEntity));
        }

        /// <summary>
        /// Creates an array of <see cref="Guid"/>s that SQL understands.
        /// </summary>
        /// <param name="ids"><see cref="List{Guid}<"/></param>
        /// <returns>SQL variant of a <see cref="Guid"/> array</returns>
        private string ComposeIdArray(List<Guid> ids)
        {
            if (ids == null) { throw new ArgumentNullException(nameof(ids)); }
            if (ids.Count == 0) { return "'{}'"; }

            var result = $"{ids[0]}";
            for (int i = 1; i < ids.Count; i++)
            {
                if (i < ids.Count) { result += ", "; }
                result += $"{ids[i]}";
            }

            return "'{" + result + "}'"; // TODO Bit messy
        }

    }

}
