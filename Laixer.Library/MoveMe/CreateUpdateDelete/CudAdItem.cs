using Laixer.Library.Injection.Database;
using Maximiz.Model.Entity;
using System;
using System.Threading.Tasks;

namespace Maximiz.Transactions.CreateUpdateDelete
{

    /// <summary>
    /// Performs CUD operations for <see cref="AdItem"/>s in our database.
    /// </summary>
    public class CudAdItem : ICud<AdItem>
    {

        /// <summary>
        /// Provides database connections for us.
        /// </summary>
        private IDatabaseProvider databaseProvider;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="provider"><see cref="IDatabaseProvider"/>.</param>
        public CudAdItem(IDatabaseProvider provider)
        {
            databaseProvider = provider;
        }

        public Task<AdItem> Create(AdItem entity)
        {
            throw new NotImplementedException();
        }

        public Task<AdItem> Update(AdItem entity)
        {
            throw new NotImplementedException();
        }

        public Task<AdItem> Delete(AdItem entity)
        {
            throw new NotImplementedException();
        }

    }
}
