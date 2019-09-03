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
        private readonly string _baseUrl;

        public string TokenUri { get; set; }
        public string RefreshUri { get; set; }
        public OAuthAuthorizationProvider AuthorizationProvider { get; set; }

        public HttpManager(string baseUrl) => _baseUrl = baseUrl;

        /// <summary>
        /// Create or reuse an OAuthHttpClient, with our
        /// tokens included.
        /// </summary>
        protected OAuthHttpClient BuildHttpClient(bool newInstance = false)
        {
            if (_client != null && !newInstance)
            {
                return _client;
            }

            return _client = new OAuthHttpClient
            {
                BaseAddress = new Uri(_baseUrl),
                TokenUri = TokenUri,
                RefreshUri = RefreshUri,
                AuthorizationProvider = AuthorizationProvider,
            };
        }

        /// <summary>
        /// Execute API query and return result. This function
        /// actually communicates with the database.
        /// </summary>
        /// <remarks>
        /// This operations throws an exception when status is 
        /// not HTTP.OK.
        /// </remarks>
        /// <param name="method">HTTP method</param>
        /// <param name="url">Endpoint</param>
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
        /// Execute API execute without retrieving result.
        /// </summary>
        /// <param name="method">HTTP method</param>
        /// <param name="url">Endpoint</param>
        public async Task RemoteExecuteAsync(string url, HttpContent content, 
            CancellationToken cancellationToken)
        {
            using (var httpResponse = await BuildHttpClient().PostAsync(url, content, cancellationToken))
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
