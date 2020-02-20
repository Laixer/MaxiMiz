using System;

namespace Poller.Scheduler.Activator
{
    public class ActivatorFactory
    {
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ActivatorFactory(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider;

        /// <summary>
        /// Instanciate the time activator.
        /// </summary>
        /// <param name="operation">Operator instance.</param>
        /// <param name="timeSpan">Time interval.</param>
        /// <returns><see cref="ActivatorBase"/>.</returns>
        public ActivatorBase TimeActivator(IOperationDelegate operation, TimeSpan timeSpan)
            => new TimerActivator(ServiceProvider, operation, timeSpan);

        /// <summary>
        /// Instanciate the event activator.
        /// </summary>
        /// <param name="operation">Operator instance.</param>
        /// <returns><see cref="ActivatorBase"/>.</returns>
        public ActivatorBase EventActivator(IOperationDelegate operation, string queueName)
            => new EventActivator(ServiceProvider, operation, queueName);
    }
}
