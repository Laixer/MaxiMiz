using Maximiz.Model.Entity;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Commiting
{

    /// <summary>
    /// Contract for committing <see cref="CampaignGroup"/>s to our data store.
    /// </summary>
    public interface ICampaignGroupCommitter : ICommitter<CampaignGroup>
    {

        Task<CampaignGroup> CreateAsyncFromConnection(IDbConnection connection, CampaignGroup campaignGroup, CancellationToken token);

    }
}
