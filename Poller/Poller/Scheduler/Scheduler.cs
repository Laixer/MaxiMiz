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

        public bool Any()
        {
            return RefreshAdvertisementDataProvider != null
                || DataSyncbackProvider != null
                || CreateOrUpdateObjectsProvider != null;
        }

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

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
