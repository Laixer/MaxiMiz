using System;
using System.Threading.Tasks;
using Google.Ads.GoogleAds;
using Google.Ads.GoogleAds.Lib;
using Google.Ads.GoogleAds.V1.Errors;
using Google.Ads.GoogleAds.V1.Services;
using Google.Api.Gax;
using Poller.Publisher;

namespace Poller.GoogleAds
{
    [Publisher("GoogleAds")]
    public class GooglePoller : IRemotePublisher
    {
        public void RunExample()
        {
            long customerId = -1L;

            Run(new GoogleAdsClient(), customerId);
        }

        /// <summary>
        /// Runs the code example.
        /// </summary>
        /// <param name="client">The Google Ads client.</param>
        /// <param name="customerId">The Google Ads customer ID for which the call is made.</param>
        public void Run(GoogleAdsClient client, long customerId)
        {
            // Get the GoogleAdsService.
            GoogleAdsServiceClient googleAdsService = client.GetService(Services.V1.GoogleAdsService);

            SearchGoogleAdsRequest request = new SearchGoogleAdsRequest()
            {
                PageSize = 1000,
                Query = @"SELECT
                            campaign.id,
                            campaign.name,
                            campaign.network_settings.target_content_network
                        FROM campaign
                        ORDER BY campaign.id",
                CustomerId = customerId.ToString()
            };

            try
            {
                // Issue the search request.
                PagedEnumerable<SearchGoogleAdsResponse, GoogleAdsRow> searchPagedResponse =
                    googleAdsService.Search(request);

                foreach (SearchGoogleAdsResponse response in searchPagedResponse.AsRawResponses())
                {
                    Console.WriteLine(response.FieldMask.Paths);
                    foreach (GoogleAdsRow googleAdsRow in response.Results)
                    {
                        Console.WriteLine("Campaign with ID {0} and name '{1}' was found.",
                            googleAdsRow.Campaign.Id, googleAdsRow.Campaign.Name);
                    }
                }
                // Iterate over all rows in all pages and prints the requested field values for the
                // campaign in each row.
                foreach (GoogleAdsRow googleAdsRow in searchPagedResponse)
                {
                    Console.WriteLine("Campaign with ID {0} and name '{1}' was found.",
                        googleAdsRow.Campaign.Id, googleAdsRow.Campaign.Name);
                }
            }
            catch (GoogleAdsException e)
            {
                Console.WriteLine("Failure:");
                Console.WriteLine($"Message: {e.Message}");
                Console.WriteLine($"Failure: {e.Failure}");
                Console.WriteLine($"Request ID: {e.RequestId}");
            }
        }

        public Task GetTopCampaignReportAsync()
        {
            return Task.CompletedTask;
        }
    }
}
