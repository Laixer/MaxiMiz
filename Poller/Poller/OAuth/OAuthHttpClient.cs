using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Timers;

public class OAuthHttpClient : HttpClient
{
    private Timer timer;

    public string AuthorizeUri { get; set; }
    public string TokenUri { get; set; }
    public string RefreshUri { get; set; }

    public OAuthHttpClient()
    {
        DefaultRequestHeaders.UserAgent.ParseAdd("Poller.Host");
    }

    public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken = default)
    {
        // check if auth
        // check if auth valid
        // attach token

        return base.SendAsync(request, cancellationToken);
    }

    protected virtual Dictionary<string, string> BuildAuthorizeRequest()
    {
        return new Dictionary<string, string>
        {
            // {OAuthGrantType.ClientId, options.OAuth2.ClientId},
            // {OAuthGrantType.ClientSecret, options.OAuth2.ClientSecret},
        };
    }

    protected async virtual Task SendAuthorizeRequestAsync(string grantType, string tokenUri = null)
    {
        using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, tokenUri ?? TokenUri)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>(BuildAuthorizeRequest())
            {
                // {OAuthGrantType.Username, options.OAuth2.Username},
                // {OAuthGrantType.Password, options.OAuth2.Password},
                // {OAuthGrantType.GrantType, grantType},
            })
        })
        using (var httpResponse = await SendAsync(httpRequest))
        {
            // set refresh timer
            // return await Json.DeserializeAsync<OAuth2Response>(httpResponse);
        }
    }

    protected virtual Task SendTokenRefreshRequestAsync()
    {
        return Task.CompletedTask;
    }
}
