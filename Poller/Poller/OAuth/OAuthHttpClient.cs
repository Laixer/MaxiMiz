using Poller.Helper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Poller.OAuth
{
    public partial class OAuthHttpClient : HttpClient
    {
        protected OAuthTicket ticket;

        public string AuthorizeUri { get; set; }
        public string TokenUri { get; set; }
        public string RefreshUri { get; set; }
        public OAuthAuthorizationProvider AuthorizationProvider { get; set; }

        public OAuthHttpClient()
        {
            DefaultRequestHeaders.UserAgent.ParseAdd("Poller.Host");
        }

        public OAuthHttpClient(OAuthAuthorizationProvider oAuthAuthorizationProvider)
            : this()
        {
            AuthorizationProvider = oAuthAuthorizationProvider;
        }

        protected async Task AttachTokenAuthentication(HttpRequestMessage httpRequest)
        {
            if (ticket == null)
            {
                ticket = await SendAuthorizeRequestAsync("password");
            }
            else if (!ticket.IsValid)
            {
                ticket = await SendTokenRefreshRequestAsync();
            }

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue(OAuthAuthenticationType.Bearer, ticket.AccessToken);
        }

        private async Task<HttpResponseMessage> SendInternalAsync(HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            await AttachTokenAuthentication(httpRequest).ConfigureAwait(false);

            return await base.SendAsync(httpRequest, cancellationToken);
        }

        /// <summary>
        /// Redirect all calls to internal sender.
        /// </summary>
        /// <param name="request">Http request, see <see cref="HttpRequestMessage"/>.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="HttpResponseMessage"/>.</returns>
        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendInternalAsync(request, cancellationToken);
        }

        /// <summary>
        /// Redirect all calls to internal sender.
        /// </summary>
        /// <param name="request">Http request, see <see cref="HttpRequestMessage"/>.</param>
        /// <returns><see cref="HttpResponseMessage"/>.</returns>
        public new Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return SendInternalAsync(request, default);
        }

        /// <summary>
        /// Build the OAuth request structure.
        /// </summary>
        /// <returns>Key value pair.</returns>
        protected virtual Dictionary<string, string> BuildAuthorizeRequest()
        {
            return new Dictionary<string, string>
            {
                {OAuthGrantType.ClientId, AuthorizationProvider.ClientId},
                {OAuthGrantType.ClientSecret, AuthorizationProvider.ClientSecret},
            };
        }

        /// <summary>
        /// Send OAuth authorize request.
        /// </summary>
        /// <param name="grantType">Grant type.</param>
        /// <param name="tokenUri">Endpoint.</param>
        /// <returns><see cref="OAuthTicket"/>.</returns>
        protected async virtual Task<OAuthTicket> SendAuthorizeRequestAsync(string grantType, string tokenUri = null)
        {
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, tokenUri ?? TokenUri)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>(BuildAuthorizeRequest())
                {
                    {OAuthGrantType.Username, AuthorizationProvider.Username},
                    {OAuthGrantType.Password, AuthorizationProvider.Password},
                    {OAuthGrantType.GrantType, grantType},
                })
            })
            using (var httpResponse = await base.SendAsync(httpRequest))
            {
                return await Json.DeserializeAsync<OAuthTicket>(httpResponse);
            }
        }

        /// <summary>
        /// Send OAuth refresh token request.
        /// </summary>
        /// <param name="grantType">Grant type.</param>
        /// <param name="tokenUri">Endpoint.</param>
        /// <returns><see cref="OAuthTicket"/>.</returns>
        protected async virtual Task<OAuthTicket> SendTokenRefreshRequestAsync(string grantType = OAuthGrantType.RefreshToken, string refreshUri = null)
        {
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, refreshUri ?? RefreshUri)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>(BuildAuthorizeRequest())
                {
                    {OAuthGrantType.RefreshToken, ticket.RefreshToken},
                    {OAuthGrantType.GrantType, grantType},
                })
            })
            using (var httpResponse = await base.SendAsync(httpRequest))
            {
                return await Json.DeserializeAsync<OAuthTicket>(httpResponse);
            }
        }
    }
}
