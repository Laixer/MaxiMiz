using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Poller.Publisher;
using Poller.Scheduler.Activator;

namespace Poller.Host
{
    /// <summary>
    /// Run the remote services.
    /// </summary>
    internal class RemoteApplicationService : IHostedService, IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ICollection<ActivatorBase> _activatorBases = new List<ActivatorBase>();

        protected ILogger Logger { get; }
        protected IServiceProvider ServiceProvider { get; }
        protected CancellationToken CancellationToken { get => _cancellationTokenSource.Token; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="services">Service provider.</param>
        public RemoteApplicationService(ILogger<RemoteApplicationService> logger, IServiceProvider services)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ServiceProvider = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Start service and schedule all operations to run.
        /// </summary>
        /// <remarks>This method will catch *all* exceptions.</remarks>
        /// <param name="token">Cancellation token.</param>
        public Task StartAsync(CancellationToken token)
        {
            if (token.IsCancellationRequested) { return Task.CompletedTask; }

            Logger.LogInformation("Services starting");

            InitializeActivators(token);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Initialize all activators.
        /// </summary>
        /// <param name="token">Cancellation Token.</param>
        private void InitializeActivators(CancellationToken token)
        {
            try
            {
                foreach (var remotePublishers in ServiceProvider.GetService<IEnumerable<IRemotePublisher>>().ToArray())
                {
                    foreach (var activator in remotePublishers.GetActivators(token).AsParallel())
                    {
                        activator.Initialize(CancellationToken);
                        _activatorBases.Add(activator);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogCritical(e.Message);
                // TODO: Request shutdown
            }
        }

        /// <summary>
        /// Terminate all activators.
        /// </summary>
        /// <param name="token">Cancellation Token.</param>
        private void TerminateActivators(CancellationToken token)
        {
            try
            {
                foreach (var activator in _activatorBases.AsParallel())
                {
                    activator.Dispose();
                }
            }
            catch (Exception e)
            {
                Logger.LogCritical(e.Message);
                // TODO: Request shutdown
            }
        }

        /// <summary>
        /// Stop service by cancelling all tokens.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        public Task StopAsync(CancellationToken token)
        {
            if (token.IsCancellationRequested) { return Task.CompletedTask; }

            Logger.LogInformation("Services stopping");

            _cancellationTokenSource.Cancel();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();

            TerminateActivators(default);
        }
    }
}
