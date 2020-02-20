using System;
using static Maximiz.Infrastructure.Querying.QueryExtractor;
using static Maximiz.Infrastructure.Querying.DatabaseColumnMap;
using Maximiz.Model.Entity;
using System.Collections.Generic;

namespace Maximiz.Infrastructure.Committing.Sql
{
    internal static partial class Sql
    {

        internal static string SqlSelectForUpdate<TEntity>(List<Guid> ids)
            where TEntity : Entity
        {
            if (ids == null) { throw new ArgumentNullException(nameof(ids)); }
            if (ids.Count == 0) { throw new InvalidOperationException(nameof(ids)); } // TODO Correct?

            var result = $"SELECT * FROM {GetTableName<TEntity>()}" +
                $" WHERE ";
            result += $" id = '{ids[0]}'";
            for (int i = 1; i < ids.Count; i++)
            {
                result += $" OR id = '{ids[i]}'";
            }
            result += " FOR UPDATE;";

            return result;
        }

    }
}
