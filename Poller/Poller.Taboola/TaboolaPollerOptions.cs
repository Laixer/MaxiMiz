using Poller.Model;

namespace Poller.Taboola
{
    /// <summary>
    /// Taboola configuration options.
    /// </summary>
    public class TaboolaPollerOptions
    {
        /// <summary>
        /// API domain.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Authentication settings.
        /// </summary>
        public OAuth2 OAuth2 { get; set; }

        /// <summary>
        /// Interval (in minutes) for operation.
        /// </summary>
        public int RefreshAdvertisementDataInterval { get; set; } = 60;

        /// <summary>
        /// Interval (in minutes) for operation.
        /// </summary>
        public int DataSyncbackInterval { get; set; } = 6 * 60;

        /// <summary>
        /// Event bus name on which events are received for backend communication.
        /// </summary>
        public string CreateOrUpdateObjectsEventBus { get; set; }
    }
}
