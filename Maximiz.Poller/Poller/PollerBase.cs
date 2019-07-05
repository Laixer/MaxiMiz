using System.Threading.Tasks;
using System.Net.Http;
using MaxiMiz.Poller.Model.Response;
using MaxiMiz.Poller.Poller.Abstract;

namespace MaxiMiz.Poller.Poller
{
    public abstract class PollerBase : IRestPoller
    {
        // TODO: This should not be part of the base, see comment in IRestPoller
        public HttpClient Client { get; protected set; }

        public abstract Task<TopCampaignReport> GetTopCampaignReportAsync();
        
        public virtual Task<string> GetOAuth2ResponseAsync()
        {
            return Task.FromResult(string.Empty);
        }

        public virtual void Dispose()
        {
            // NOTE: Since HttpClient is class global we need to call dispose
            Client?.Dispose();
        }
    }
}
