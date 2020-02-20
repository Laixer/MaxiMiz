using Maximiz.Model;
using Maximiz.Model.Entity;

namespace Poller.Scheduler
{
    public class EventActivatorOperationContext : IOperationContext
    {
        /// <summary>
        /// Entity.
        /// </summary>
        public Entity[] Entity { get; set; }

        /// <summary>
        /// Entity action.
        /// </summary>
        public CrudAction EntityAction { get; set; }
    }
}
