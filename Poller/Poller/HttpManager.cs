using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Poller.Helper;
using Poller.OAuth;

namespace Poller
{

    /// <summary>
    /// Manages all our http requests. This class does not
    /// sends the requests itself, but delegates this to the
    /// <see cref="OAuthHttpClient"/> class.
    /// </summary>
    public class HttpManager : IDisposable
    {

        /// <summary>
        /// Static singleton client to manage authentication.
        /// </summary>
        private static OAuthHttpClient _client;

        /// <summary>
        /// Contains urls and endpoints.
        /// </summary>
        private readonly Uris _uris;

        /// <summary>
        /// Contains our client secrets and credentials.
        /// </summary>
        private readonly OAuthAuthorizationProvider _authorizationProvider;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="uris">Contains our url 
        /// and endpoints</param>
        /// <param name="authorizationProvider">
        /// Contains our client credentials</param>
        public HttpManager(Uris uris, OAuthAuthorizationProvider authorizationProvider)
        {
            _uris = uris;
            _authorizationProvider = authorizationProvider;
        }

        /// <summary>
        /// Create or reuse an OAuthHttpClient, with our
        /// tokens included.
        /// </summary>
        /// <param name="newInstance">Set to true to force
        /// the creation of a new authorization client</param>
        /// <returns>The created client.</returns>
        internal OAuthHttpClient BuildAuthorizedHttpClient(bool newInstance = false)
        {
            if (_client != null && !newInstance)
            {
                return _client;
            }

            return _client = new OAuthHttpClient(_uris, _authorizationProvider);
        }

        /// <summary>
        /// Execute API query and return result. This function calls the
        /// <see cref="OAuthHttpClient"/> which actually communicates with
        /// the internet.
        /// </summary>
        /// <remarks>
        /// This operations throws an exception when status is not HTTP.OK.
        /// </remarks>
        /// <param name="method">HTTP method</param>
        /// <param name="endpoint">Endpoint</param>
        /// <param name="cancellationToken">Cancellation otken</param>
        public async Task<TResult> RemoteQueryAsync<TResult>(
            HttpMethod method, string url, CancellationToken cancellationToken)
            where TResult : class
        {
            using (var httpResponse = await BuildHttpClient().SendAsync(new HttpRequestMessage(method, url), cancellationToken))
            {
                httpResponse.EnsureSuccessStatusCode();
                return await Json.DeserializeAsync<TResult>(httpResponse);
            }
        }

        /// <summary>
        /// API execution.
        /// 
        /// TODO What can our status codes be?
        /// </summary>
        public async Task RemoteExecuteAsync(string url, HttpContent content, 
        /// <param name="method">Http method</param>
        /// <param name="endpoint">Endpoint url without the base</param>
        /// <param name="content">Http content</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
            CancellationToken cancellationToken)
        {
            using (var httpResponse = await BuildAuthorizedHttpClient().PostAsync(
                url, content, cancellationToken))
            {
                httpResponse.EnsureSuccessStatusCode();
            }
        }

        /// <summary>
        /// Dispose objects.
        /// </summary>
        public void Dispose() => _client?.Dispose();
    }
}
