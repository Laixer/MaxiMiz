using System;
using Maximiz.Model;
using Maximiz.Model.Entity;

namespace Poller.Poller
{

    /// <summary>
    /// Contains the type of operation a poller should execute with regards 
    /// to modifying entities in external APIs.
    /// </summary>
    public class CreateOrUpdateObjectsContext : PollerContext
    {

        /// <summary>
        /// Action on entity.
        /// </summary>
        public CrudAction Action { get; set; }

        /// <summary>
        /// Entity to process.
        /// </summary>
        public Entity[] Entity { get; set; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="runCount">Run count.</param>
        /// <param name="lastRun">Last run.</param>
        /// <param name="progressCallback">Process callback.</param>
        public CreateOrUpdateObjectsContext(int runCount, DateTime? lastRun, Action progressCallback = null)
            : base(runCount, lastRun, progressCallback)
        {
        }

    }

}
