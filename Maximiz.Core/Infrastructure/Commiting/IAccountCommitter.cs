using Maximiz.Model.Entity;

namespace Maximiz.Core.Infrastructure.Commiting
{

    /// <summary>
    /// Contract for committing <see cref="Account"/>s to our data store.
    /// </summary>
    public interface IAccountCommitter : ICommitter<Account>, IBulkCommitter<Account>
    {

        //

    }
}
