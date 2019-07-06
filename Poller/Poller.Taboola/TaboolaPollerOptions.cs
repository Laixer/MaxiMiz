using Poller.Model;

namespace Poller.Taboola
{
    public class TaboolaPollerOptions
    {
        /// <summary>
        /// API domain.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Account identifier.
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// Authentication settings.
        /// </summary>
        public OAuth2 OAuth2 { get; set; }
    }
}
