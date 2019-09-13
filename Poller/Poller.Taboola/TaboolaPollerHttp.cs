using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Poller.Taboola
{

    /// <summary>
    /// This is the part of our Taboola Poller that uses http. All outgoing 
    /// requests are placed within this file. All http requests are handled
    /// by our <see cref="HttpManager"/>.
    /// </summary>
    internal partial class TaboolaPoller
    {

        /// <summary>
        /// Run the remote query and catch all exceptions  before letting them 
        /// propagate upwards.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="method">HTTP method.</param>
        /// <param name="endpoint">API endpoint.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task with TResult object</returns>
        protected async Task<TResult> RemoteQueryAndLogAsync
            <TResult>(HttpMethod method, string endpoint,
            CancellationToken cancellationToken)
            where TResult : class
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                _logger.LogTrace($"Querying {method} {endpoint}");

                return await _client.RemoteQueryAsync<TResult>(method, endpoint, cancellationToken);
            }
            catch (Exception e) when (e as OperationCanceledException == null && e as TaskCanceledException == null)
            {
                _logger.LogError($"{endpoint}: {e.Message}");
                throw e;
            }
        }

        /// <summary>
        /// Run the remote execute and catch all exceptions where before letting
        /// them propagate upwards. This is typeless and returns nothing.
        /// </summary>
        /// <param name="method">Http method</param>
        /// <param name="endpoint">API endpoint</param>
        /// <param name="content">Http content</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        protected async Task RemoteExecuteAndLogAsync(HttpMethod method,
            string endpoint, HttpContent content, CancellationToken cancellationToken)
        {
            await RemoteExecuteAndLogAsync<object>(method, endpoint, content, cancellationToken);
        }

        /// <summary>
        /// Run the remote execute and catch all exceptions where before letting
        /// them propagate upwards.
        /// </summary>
        /// <typeparam name="TResult">Expected return type</typeparam>
        /// <param name="method">Http method</param>
        /// <param name="endpoint">API endpoint</param>
        /// <param name="content">Http content</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        protected async Task<TResult> RemoteExecuteAndLogAsync<TResult>(
            HttpMethod method, string endpoint, HttpContent content,
            CancellationToken cancellationToken)
            where TResult : class
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var contentString = "no content";
                if (content != null) { contentString = content.ReadAsStringAsync().ToString(); }
                _logger.LogTrace($"Executing {endpoint} with content {contentString}");

                return await _client.RemoteExecuteAsync<TResult>(
                    method, endpoint, content, cancellationToken);
            }
            catch (Exception e) when (e as OperationCanceledException == null
            && e as TaskCanceledException == null)
            {
                _logger.LogError($"{endpoint}: {e.Message}");
                throw e;
            }
        }

    }
}
