using System.Collections;
using System.Collections.Generic;
using Poller.Scheduler.Delegate;

namespace Poller.Scheduler
{
    public sealed class ScheduleCollection : IEnumerable<IOperationDelegate>
    {
        public RefreshAdvertisementDataDelegate RefreshAdvertisementDataProvider { get; set; }
        public DataSyncbackDelegate DataSyncbackProvider { get; set; }
        public CreateOrUpdateObjectsDelegate CreateOrUpdateObjectsProvider { get; set; }

        /// <summary>
        /// Test if any data provider is set.
        /// </summary>
        /// <returns>Bool if at least one data provider is set.</returns>
        public bool Any()
        {
            return RefreshAdvertisementDataProvider != null
                || DataSyncbackProvider != null
                || CreateOrUpdateObjectsProvider != null;
        }

        /// <summary>
        /// Return the data providers as an ennumerable collection.
        /// </summary>
        /// <returns><see cref="IEnumerator<IOperationDelegate>"/>.</returns>
        public IEnumerator<IOperationDelegate> GetEnumerator()
        {
            if (RefreshAdvertisementDataProvider != null)
            {
                yield return RefreshAdvertisementDataProvider;
            }
            if (DataSyncbackProvider != null)
            {
                yield return DataSyncbackProvider;
            }
            if (CreateOrUpdateObjectsProvider != null)
            {
                yield return CreateOrUpdateObjectsProvider;
            }
        }

        /// <summary>
        /// Return the data providers as an ennumerable collection.
        /// </summary>
        /// <returns><see cref="IEnumerator"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
