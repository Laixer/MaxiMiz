namespace Poller.Host
{
    /// <summary>
    /// Remote application service options.
    /// </summary>
    public sealed class RemoteApplicationServiceOptions
    {
        /// <summary>
        /// Delay in seconds before starting the service.
        /// </summary>
        public int StartupDelay { get; set; } = 5;

        /// <summary>
        /// Publisher running time in minutes before operation is cancelled.
        /// </summary>
        public int PublisherOperationTimeout { get; set; } = 10;
    }
}
