using Maximiz.Model.Entity;

namespace Maximiz.Core.Infrastructure.Commiting
{

    /// <summary>
    /// Contract for committing <see cref="Campaign"/>s to our data store.
    /// </summary>
    public interface ICampaignCommitter : ICommitter<Campaign>, IBulkCommitter<Campaign>
    {

        //

    }
}
