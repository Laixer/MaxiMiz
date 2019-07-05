namespace MaxiMiz.Poller.Model.Config
{
    // TODO: All items should be properties
    internal class OAuth2Config
    {
        public OAuth2Config(string OAuth2ClientId, string OAuth2ClientSecret, string OAuth2RefreshToken, string OAuth2AccessToken, string grantType, string username = "", string password = "")
        {
            this.OAuth2ClientId = OAuth2ClientId;
            this.OAuth2ClientSecret = OAuth2ClientSecret;
            this.OAuth2RefreshToken = OAuth2RefreshToken;
            this.OAuth2AccessToken = OAuth2AccessToken;
            this.grantType = grantType;
            this.username = username;
            this.password = password;
        }

        internal readonly string OAuth2ClientId;

        internal readonly string OAuth2ClientSecret;

        internal string OAuth2RefreshToken;

        internal string OAuth2AccessToken;

        internal readonly string grantType;

        internal string username;

        internal string password;
    }
}