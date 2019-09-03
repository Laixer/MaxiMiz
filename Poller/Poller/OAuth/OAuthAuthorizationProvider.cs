namespace Poller.OAuth
{

    /// <summary>
    /// This contains our credentials for the
    /// OAuth authorization.
    /// </summary>
    public class OAuthAuthorizationProvider
    {
        /// <summary>
        /// Client identifier.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Authentication username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Authentication password.
        /// </summary>
        public string Password { get; set; }
    }
}
