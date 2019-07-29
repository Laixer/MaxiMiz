using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Poller.Extensions;

namespace Poller.Scheduler.Activator
{
    public abstract class ActivatorBase : IDisposable
    {
        public IOperationDelegate Operation { get; }
        public IServiceProvider ServiceProvider { get; set; }
        protected ILogger Logger { get; }

        protected CancellationToken CancellationToken { get; set; } = default;
        protected string OperationName { get => Operation.GetType().FullName; }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="operation">Delegate operation.</param>
        public ActivatorBase(IServiceProvider serviceProvider, IOperationDelegate operation)
        {
            ServiceProvider = serviceProvider;
            Operation = operation;
            Logger = ServiceProvider.GetRequiredService<ILogger<ActivatorBase>>();
        }

        /// <summary>
        /// Log the exception.
        /// </summary>
        /// <param name="e">Exception.</param>
        private void LogException(Exception e)
        {
            Logger.LogError(e.Message);
            Logger.LogTrace(e.StackTrace);
        }

        /// <summary>
        /// Run an operation delegate.
        /// </summary>
        protected void ExecuteProvider()
        {
            if (CancellationToken.IsCancellationRequested) { return; }

            var watch = new Stopwatch();

            try
            {
                Logger.LogInformation($"Start {OperationName}");
                Logger.LogDebug($"Start {OperationName} at {DateTime.Now}");

                watch.Start();

                // Link the cancellation tokens into one new cancellation token, but only for local scope.
                using (var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(CancellationToken))
                using (var deadlineTimer = new DeadlineTimer(combinedCts))
                {
                    combinedCts.Token.Register(() => Logger.LogWarning("Operation timeout or canceled, task killed"));

                    void SetDeadlineTimer()
                    {
                        // _options.PublisherOperationTimeout
                        // Use provider timespan if timeout is set by provider.
                        deadlineTimer.Reset(Operation.Timeout.TotalMilliseconds > 0
                            ? Operation.Timeout
                            : TimeSpan.FromMinutes(5));
                    }

                    // Restart the timeout from this point.
                    Operation.OnSlidingWindowChange += (s, e) => SetDeadlineTimer();

                    SetDeadlineTimer();

                    // Run operation synchronous.
                    Operation.InvokeAsync(combinedCts.Token).Wait(combinedCts.Token);
                }
            }
            catch (AggregateException e)
            {
                e.Handle((_e) =>
                {
                    // Ignore cancelation related exceptions.
                    if (_e is TaskCanceledException || _e is OperationCanceledException)
                    {
                        return true;
                    }

                    LogException(_e);
                    return true;
                });
            }
            catch (Exception e) when (e as OperationCanceledException == null)
            {
                LogException(e);
            }
            catch { }
            finally
            {
                watch.Stop();

                // TODO: This is ugly
                Operation.UnsubscribeAllEvents();

                Logger.LogInformation($"Finished {OperationName}");
                Logger.LogDebug($"Finished {OperationName} in {watch.Elapsed}");
            }
        }

        public abstract void Initialize(CancellationToken token);

        public virtual void Dispose() { }
    }
}
