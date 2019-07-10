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
    }
}
