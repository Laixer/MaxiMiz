using Poller.Helper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Poller.OAuth
{

    /// <summary>
    /// This actually sends our requests with integrated
    /// authorization.
    /// </summary>
    internal class OAuthHttpClient : HttpClient
    {

        /// <summary>
        /// The authorization ticket containing our tokens.
        /// </summary>
        private OAuthTicket _ticket;

        /// <summary>
        /// Contains our url and token endpoints.
        /// </summary>
        private readonly Uris _uris;

        private async Task<HttpResponseMessage> SendInternalAsync(HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            await AttachTokenAuthentication(httpRequest).ConfigureAwait(false);
        /// <summary>
        /// Contains our client secrets and credentials.
        /// </summary>
        private readonly OAuthAuthorizationProvider _authorizationProvider;

            return await base.SendAsync(httpRequest, cancellationToken);
        }

        /// <summary>
        /// Redirect all calls to internal sender.
        /// Constructor which sets our default headers.
        /// </summary>
        /// <param name="request">Http request, see <see cref="HttpRequestMessage"/>.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="HttpResponseMessage"/>.</returns>
        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        /// <param name="uris">Base url and token endpoints</param>
        public OAuthHttpClient(Uris uris, OAuthAuthorizationProvider authorizationProvider)
        {
            return SendInternalAsync(request, cancellationToken);
            _uris = uris;
            _authorizationProvider = authorizationProvider;
            DefaultRequestHeaders.UserAgent.ParseAdd("Poller.Host");

            _credentials = new Dictionary<string, string>
            {
                {OAuthGrantType.ClientId, _authorizationProvider.ClientId},
                {OAuthGrantType.ClientSecret, _authorizationProvider.ClientSecret},
            };
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
        /// This checks if our authorization ticket exists and 
        /// is still valid and gets us a new one if it isn't.
        /// After that the ticket is attached to the request.
        /// </summary>
        /// <returns>Key value pair.</returns>
        protected virtual Dictionary<string, string> BuildAuthorizeRequest()
        /// <param name="httpRequest">The request</param>
        /// <returns>The request with authentication</returns>
        protected async Task AttachTokenAuthentication(HttpRequestMessage httpRequest)
        {
            if (_ticket == null)
            {
                {OAuthGrantType.ClientId, AuthorizationProvider.ClientId},
                {OAuthGrantType.ClientSecret, AuthorizationProvider.ClientSecret},
            };
                _ticket = await SendAuthorizeRequestAsync("password");
            }
            else if (!_ticket.IsValid)
            {
                _ticket = await SendTokenRefreshRequestAsync();
            }

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue(
                OAuthAuthenticationType.Bearer, _ticket.AccessToken);
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
