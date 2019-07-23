﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Poller.Poller;

namespace Poller.Scheduler.Delegate
{
    public class RefreshAdvertisementDataDelegate : OperationDelegate<IPollerRefreshAdvertisementData>
    {
        public RefreshAdvertisementDataDelegate(IPollerRefreshAdvertisementData poller, TimeSpan timeSpan)
            : base(poller, timeSpan)
        {
        }

        protected override async Task InvokeDelegateAsync(CancellationToken token)
        {
            var pollerContext = BuildPollerContext();

            await _poller.RefreshAdvertisementDataAsync(pollerContext, token);

            DigestPollerContext(pollerContext);
        }
    }
}
