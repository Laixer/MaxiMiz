using System;
using System.Data.Common;
using System.Threading.Tasks;
using Google.Ads.GoogleAds;
using Google.Ads.GoogleAds.Lib;
using Google.Ads.GoogleAds.V1.Services;
using Google.Api.Gax;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Poller.Model.Response;
using Poller.Publisher;
using System.Linq;

namespace Poller.GoogleAds
{
    [Publisher("GoogleAds")]
    public class GoogleAdsPoller : RemotePublisher
    {
        private readonly Lazy<GoogleAdsClient> _client;

        private readonly DbConnection connection;

        private readonly GoogleAdsPollerOptions options;

        protected GoogleAdsClient client { get => _client.Value; }

        /// <summary>
        /// Creates a TaboolaPoller for fetching Data from Taboola.
        /// </summary>
        /// <param name="logger">A logger for this poller.</typeparam>
        /// <param name="options">An instance of options required for requests.</param>
        /// <param name="connection">The database connections to use for inserting fetched data.</param>
        public GoogleAdsPoller(ILogger<GoogleAdsPoller> logger, IOptions<GoogleAdsPollerOptions> options, DbConnection connection)
            : base(logger)
        {
            this.options = options?.Value;
            this.connection = connection;

            // Lazy initialization prevent performance hit on process start.
            _client = new Lazy<GoogleAdsClient>(() => new GoogleAdsClient(options.Value.Config));
        }


        public async override Task GetTopCampaignReportAsync()
        {
            GoogleAdsServiceClient googleAdsService = client.GetService(Services.V1.GoogleAdsService);
            SearchGoogleAdsRequest request = new SearchGoogleAdsRequest()
            {
                PageSize = 1000,
                Query = @"SELECT
	                        ad_group_ad.ad.display_url,
	                        campaign.name,
	                        campaign.id,
	                        ad_group_ad.ad.id,
	                        ad_group_ad.ad.url_collections,
	                        metrics.clicks,
	                        metrics.impressions,
	                        metrics.cost_micros
                        FROM ad_group_ad",
                CustomerId = getAccount(),
            };

            //TODO get accountid from endpoint.
            string getAccount()
            {
                //https://developers.google.com/google-ads/api/docs/account-management/overview
                throw new NotImplementedException();
            }

            PagedEnumerable<SearchGoogleAdsResponse, GoogleAdsRow> searchPagedResponse =
                            googleAdsService.Search(request); // TODO change to SearchAsync

            var results = searchPagedResponse.Select(row =>
            {
                var ad = row.AdGroupAd.Ad;
                var campaign = row.Campaign;
                var metrics = row.Metrics;

                try
                {
                    return new PublisherItem
                    {
                        PublisherItemId = ad.Id.Value,
                        Campaign = campaign.Id.Value,
                        CampaignName = campaign.Name,
                        ContentUrl = ad.ImageAd.ImageUrl,
                        Url = ad.DisplayUrl, //TODO is this the correct one?
                        Clicks = metrics.Clicks.Value,
                        Impressions = metrics.Impressions.Value,
                        Spent = Convert.ToDecimal(metrics.CostMicros.Value) / 1000000M,
                        Currency = null, // TODO fetch from account that is fetching this data.
                        Actions = Convert.ToInt64(metrics.Conversions.Value)
                    };
                }
                catch (NullReferenceException nre)
                {   //TODO finer error control so it does not do nothing when for example metrics is null.
                    Logger.LogError($"Attribute from server was null, item could not be created");
                    throw nre;
                };
            }).ToList();
        }
    }
}

