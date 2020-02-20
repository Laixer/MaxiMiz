using Maximiz.Model.Entity;

namespace Maximiz.Core.Infrastructure.Commiting
{

    /// <summary>
    /// Contact for committing <see cref="AdItem"/>s to our data store.
    /// </summary>
    public interface IAdItemCommitter : ICommitter<AdItem>, IBulkCommitter<AdItem>
    {

        //

    }
}
