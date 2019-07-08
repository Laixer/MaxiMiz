namespace Poller.OAuth
{
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
