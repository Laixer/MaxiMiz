using Dapper;
using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Core.Utility;
using Maximiz.Model;
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

namespace Maximiz.Infrastructure.Committing
{

    /// <summary>
    /// Contains the partial functionality for the <see cref="IOperationItemCommitter.PrepareAllAsync(MyOperation, CancellationToken)"/> function.
    /// </summary>
    public sealed partial class OperationItemCommitter : IOperationItemCommitter
    {

        /// <summary>
        /// Creates, updates and marks all <see cref="Entity"/>s in our database.
        /// </summary>
        /// <param name="operation"><see cref="MyOperation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        private Task PrepareAllAsync(IDbConnection connection, MyOperation operation, CancellationToken token)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(operation)); }
            if (token == null) { throw new ArgumentNullException(nameof(token)); }

            switch (operation.TopEntity)
            {
                case Campaign _:
                    throw new NotImplementedException();
                case CampaignGroup _:
                    return PrepareCampaignGroupAsync(connection, operation, token);
                case AdItem _:
                    throw new NotImplementedException();
                case AdGroup _:
                    throw new NotImplementedException();
            }

            throw new InvalidOperationException(nameof(operation.TopEntity));
        }

        /// <summary>
        /// Prepares an <see cref="MyOperation"/> where the top entity is of
        /// type <see cref="CampaignGroup"/>.
        /// </summary>
        /// <param name="operation"><see cref="MyOperation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        private async Task PrepareCampaignGroupAsync(IDbConnection connection, MyOperation operation, CancellationToken token)
        {
            if (operation == null) { throw new ArgumentNullException(nameof(token)); }
            if (token == null) { throw new ArgumentNullException(nameof(token)); }
            var topEntity = (operation.TopEntity as CampaignGroup) ?? throw new InvalidOperationException(nameof(operation.TopEntity));

            // Create the top campaign group if required
            if (operation.CrudAction == CrudAction.Create)
            {
                topEntity.OperationItemStatus = OperationItemStatus.PendingCreate;
                topEntity.OperationId = operation.Id;
                await _campaignGroupCommitter.CreateAsyncFromConnection(connection, topEntity, token);
            }

            // Process the operation linking entries for CG * AG
            await ProcessCGAGLinks(connection, topEntity.Id, operation.AdGroupCampaignGroupLinksAdd.ToList(), token);
            await ProcessCGAGUnlinks(connection, topEntity.Id, operation.AdGroupCampaignGroupLinksRemove.ToList(), token);

            // The database automatically inserts all C * AG links based on the CG * AG table
            if (operation.CrudAction == CrudAction.Create)
            {
                await _campaignCommitter.CreateBulkAsync(CampaignGroupExpander.Expand(topEntity), token, connection);
            }

            if (operation.CrudAction == CrudAction.Update)
            {
                // Update CG
                // Update C from CG
                throw new NotImplementedException("Campaign Group Update operation not yet implemented");
            }

            if (operation.CrudAction == CrudAction.Delete)
            {
                // Delete CG
                // Delete C
                // Delete A
                throw new NotImplementedException("Campaign Group Delete operation not yet implemented");
            }
        }

        /* TODO DRY */
        /* TODO Hard coded database columns */

        private Task ProcessCGAGLinks(IDbConnection connection, Guid campaignGroupId, IList<AdGroupCampaignGroupLinkingEntry> list, CancellationToken token)
        {
            if (list.Count == 0) { return Task.CompletedTask; }

            var sql = $"INSERT INTO public.campaign_group_ad_group (campaign_group_id, ad_group_id) VALUES ";
            for (int i = 0; i < list.Count; i++)
            {
                sql += $"('{campaignGroupId}', '{list[i].AdGroupId}')";
                if (i < list.Count - 1) { sql += ","; }
            }
            sql += ";";
            return connection.ExecuteAsync(new CommandDefinition(sql, token));
        }

        private Task ProcessCGAGUnlinks(IDbConnection connection, Guid campaignGroupId, IList<AdGroupCampaignGroupLinkingEntry> list, CancellationToken token)
        {

            if (list.Count == 0) { return Task.CompletedTask; }
            else
            {
                // TODO Also delete the C * AG entries --> how we do this?
                // TODO Also delete the ad items --> how we do this?
                throw new NotImplementedException("Unlinking Campaign Groups with Ad Groups isn't fully implemented yet");
            }

            var sql = $"REMOVE FROM public.campaign_group_ad_group WHERE";
            for (int i = 0; i < list.Count; i++)
            {
                sql += $"(campaign_group_id = '{campaignGroupId}' AND ad_group_id = '{list[i].AdGroupId}')"; // TODO HArd coded
                if (i < list.Count - 1) { sql += " OR "; }
            }
            sql += ";";
            return connection.ExecuteAsync(new CommandDefinition(sql, token));
        }

        private Task ProcessCAGLinks(List<AdGroupCampaignLinkingEntry> list, CancellationToken token)
        {
            if (list.Count == 0) { return Task.CompletedTask; }
            using (var connection = _databaseProvider.GetConnectionScope())
            {
                var sql = $"INSERT INTO public.campaign_ad_group (campaign_id, ad_group_id) VALUES ";
                for (int i = 0; i < list.Count; i++)
                {
                    sql += $"('{list[i].LinkedId}', '{list[i].AdGroupId}')";
                    if (i < list.Count - 1) { sql += ","; }
                }
                sql += ";";
                return connection.ExecuteAsync(new CommandDefinition(sql, token));
            }
        }

        private Task ProcessCAGUnlinks(List<AdGroupCampaignLinkingEntry> list, CancellationToken token)
        {
            if (list.Count == 0) { return Task.CompletedTask; }
            using (var connection = _databaseProvider.GetConnectionScope())
            {
                var sql = $"REMOVE FROM public.campaign_ad_group WHERE";
                for (int i = 0; i < list.Count; i++)
                {
                    sql += $"(campaign_id = '{list[i].LinkedId}' AND ad_group_id = '{list[i].AdGroupId}')"; // TODO Hard coded
                    if (i < list.Count - 1) { sql += " OR "; }
                }
                sql += ";";
                return connection.ExecuteAsync(new CommandDefinition(sql, token));
            }
        }

    }
}
