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
        /// Publisher refresh interval in minutes.
        /// </summary>
        public int PublisherRefreshInterval { get; set; } = 60;

        /// <summary>
        /// Publisher running time in minutes before operation is cancelled.
        /// </summary>
        public int PublisherOperationTimeout { get; set; } = 10;
    }
}
