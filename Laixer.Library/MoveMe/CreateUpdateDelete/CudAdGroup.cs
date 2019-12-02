using Laixer.Library.Injection.Database;
using Maximiz.Model.Entity;
using System;
using System.Threading.Tasks;

namespace Maximiz.Transactions.CreateUpdateDelete
{

    /// <summary>
    /// Performs CUD operations for <see cref="AdGroup"/>s in our database.
    /// </summary>
    public class CudAdGroup : ICud<AdGroup>
    {

        /// <summary>
        /// Provides database connections for us.
        /// </summary>
        private IDatabaseProvider databaseProvider;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="provider"><see cref="IDatabaseProvider"/>.</param>
        public CudAdGroup(IDatabaseProvider provider)
        {
            databaseProvider = provider;
        }

        public Task<AdGroup> Create(AdGroup entity)
        {
            throw new NotImplementedException();
        }

        public Task<AdGroup> Update(AdGroup entity)
        {
            throw new NotImplementedException();
        }

        public Task<AdGroup> Delete(AdGroup entity)
        {
            throw new NotImplementedException();
        }
    }
}
