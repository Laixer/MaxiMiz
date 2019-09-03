using System;
using System.Collections.Generic;
using System.Text;

namespace Poller
{

    /// <summary>
    /// Elegant data storage method for our uri's,
    /// endpoints and base url.
    /// </summary>
    public class Uris
    {

        /// <summary>
        /// Base url, we add to this.
        /// </summary>
        public readonly string BaseUrl;

        /// <summary>
        /// Access token endpoint addon.
        /// </summary>
        public readonly string TokenEndpoint;

        /// <summary>
        /// Refresh token endpoint addon.
        /// </summary>
        public readonly string RefreshEndpoint;

        /// <summary>
        /// Constructor which implements proper formatting.
        /// </summary>
        /// <param name="baseUrl">Base url, to which the
        /// endpoints will be added</param>
        /// <param name="tokenEndpoint">Token endpoint</param>
        /// <param name="refreshEndpoint">Refresg endpoint</param>
        public Uris(string baseUrl, string tokenEndpoint, string refreshEndpoint)
        {
            BaseUrl = new Uri(baseUrl).ToString();
            TokenEndpoint = tokenEndpoint;
            RefreshEndpoint = refreshEndpoint;
        }
               
    }
}
