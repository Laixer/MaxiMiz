using Maximiz.Model.Entity;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Commiting
{

    /// <summary>
    /// Contract for committing <see cref="Campaign"/>s to our data store.
    /// </summary>
    public interface ICampaignCommitter : ICommitter<Campaign>, IBulkCommitter<Campaign>
    {

        Task CreateBulkAsync(IEnumerable<Campaign> campaigns, CancellationToken token, IDbConnection connection);

    }
}
