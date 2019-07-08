using System;
using System.Net.Http;
using System.Threading.Tasks;
using Poller.Helper;
using Poller.OAuth;

namespace Poller
{
    public class HttpManager : IDisposable
    {
        private static OAuthHttpClient _client;
        private readonly string _baseUrl;
        public string TokenUri { get; set; }
        public string RefreshUri { get; set; }
        public OAuthAuthorizationProvider AuthorizationProvider { get; set; }

        public HttpManager(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Create or reuse an OAuthHttpClient.
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
        /// Execute API call and return result.
        /// </summary>
        /// <param name="method">HTTP method.</param>
        /// <param name="url">Endpoint.</param>
        public async Task<TResult> RemoteQueryAsync<TResult>(HttpMethod method, string url)
            where TResult : class
        {
            using (var httpResponse = await BuildHttpClient().SendAsync(new HttpRequestMessage(method, url)))
            {
                httpResponse.EnsureSuccessStatusCode();
                return await Json.DeserializeAsync<TResult>(httpResponse);
            }
        }

        /// <summary>
        /// Execute API call without expecting result.
        /// </summary>
        /// <param name="method">HTTP method.</param>
        /// <param name="url">Endpoint.</param>
        public async Task RemoteExecuteAsync(HttpMethod method, string url) 
        {
            using (var httpResponse = await BuildHttpClient().SendAsync(new HttpRequestMessage(method, url)))
            {
                httpResponse.EnsureSuccessStatusCode();
            }
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
