using System.Runtime.Serialization;

namespace MaxiMiz.Poller.Model.Response
{
    [DataContract]
    internal class OAuth2Response
    {
        public OAuth2Response(string accessToken, string tokenType, string refreshToken, int expiresIn)
        {
            this.accessToken = accessToken;
            this.tokenType = tokenType;
            this.refreshToken = refreshToken;
            this.expiresIn = expiresIn;
        }

        [DataMember(Name = "access_token")]
        internal readonly string accessToken;
        
        [DataMember(Name = "token_type")]
        internal readonly string tokenType;

        [DataMember(Name = "refresh_token")]
        internal readonly string refreshToken;

        [DataMember(Name = "expires_in")]
        internal readonly int expiresIn;
    }
}