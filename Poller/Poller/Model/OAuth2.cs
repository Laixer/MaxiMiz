namespace Poller.Model
{
    public class OAuth2
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

        /// <summary>
        /// Grant type.
        /// </summary>
        public string GrantType { get; set; }
    }
}
