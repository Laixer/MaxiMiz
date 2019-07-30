using System;
using Maximiz.Model.Entity;
using Maximiz.Model.Protocol;

namespace Poller.Poller
{
    public class CreateOrUpdateObjectsContext : PollerContext
    {
        /// <summary>
        /// Action on entity.
        /// </summary>
        public InternalMqMessage.Action Action { get; set; }

        /// <summary>
        /// Entity to process.
        /// </summary>
        public Entity Entity { get; set; }

        public CreateOrUpdateObjectsContext(int runCount, DateTime? lastRun, Action progressCallback = null)
            : base(runCount, lastRun, progressCallback)
        {
        }
    }
}
