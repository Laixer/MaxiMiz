using System.Runtime.Serialization;

namespace model.config
{
    internal class OAuth2Config
    {
        internal readonly string OAuth2ClientId;

        internal readonly string OAuth2ClientSecret;

        internal string OAuth2RefreshToken;

        internal string OAuth2AccessToken;
    }
}