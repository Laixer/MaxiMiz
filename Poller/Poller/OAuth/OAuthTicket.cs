using System;
using System.Runtime.Serialization;

namespace Poller.OAuth
{
    [DataContract]
    public class OAuthTicket
    {
        private DateTime expireTime;
        private int expireSeconds;

        /// <summary>
        /// The acces token used for requests.
        /// </summary>
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// The token type e.g. bearer
        /// </summary>
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// The token used for refreshing the access token
        /// </summary>
        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Amount of time in seconds from which the token was issued until it expires.
        /// </summary>
        [DataMember(Name = "expires_in")]
        public int ExpiresIn
        {
            get => expireSeconds;
            set
            {
                expireSeconds = value;
                expireTime = DateTime.Now.AddSeconds(expireSeconds / 2);
            }
        }

        /// <summary>
        /// Indication if ticket is still valid.
        /// </summary>
        public bool IsValid => DateTime.Now < expireTime;
    }
}
