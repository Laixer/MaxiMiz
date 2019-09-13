using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Poller.Database;
using Poller.Extensions;
using Poller.OAuth;
using Poller.Poller;
using System.Collections.Generic;

using AccountEntity = Maximiz.Model.Entity.Account;
using CampaignEntity = Maximiz.Model.Entity.Campaign;
using AdItemEntity = Maximiz.Model.Entity.AdItem;
using Poller.Taboola.Mapper;
using Poller.Taboola.Model;
using System.Collections.Generic;
using Maximiz.Model.Entity;

namespace Poller.Taboola
{

    /// <summary>
    /// Partial class for our Taboola Poller. This
    /// part implements all required activator base
    /// interfaces.
    /// </summary>
    internal partial class TaboolaPoller
    {
     

        /// <summary>
        /// Remove all created dummy campaigns. Use this after a debug session
        /// in which a lot of dummy campaigns were created.
        /// TODO This seems to be very non-parallel.
        /// </summary>
        private async Task RemoveAllDummyCampaignsAsync()
        {
            var token = new CancellationTokenSource().Token;
            var accounts = await FetchAdvertiserAccounts(token);
            var allDummies = new List<Model.Campaign>();
            foreach (var account in accounts.AsParallel())
            {
                var campaigns = await GetAllCampaigns(account, token);
                var dummies = new List<Model.Campaign>();
                dummies.AddRange(campaigns.Items.Where(x => x.Name.Equals("Dummy campaign name")).ToList());
                allDummies.AddRange(dummies);
                foreach (var dummy in dummies.AsParallel())
                {
                   await DeleteCampaignAsync(account, _mapperCampaign.Convert(dummy), token);
                }
            }
        }

        private async Task RemoveAccountDummyCampaignsAsync(AccountEntity account, CancellationToken token)
        {
            var campaigns = await GetAllCampaigns(account, token);
            var dummies = new List<Model.Campaign>();
            dummies.AddRange(campaigns.Items.Where(x => x.Name.Equals("Dummy campaign name")).ToList());
            foreach (var dummy in dummies.AsParallel())
            {
                await DeleteCampaignAsync(account, _mapperCampaign.Convert(dummy), token);
            }
        }

        /// <summary>
        /// DEBUG FUNCTION
        /// </summary>
        private async void DriveCrud()
        {
            try
            {
                // Get cancellation token
                var source = new CancellationTokenSource();
                var token = source.Token;

                // Build and commit dummy campaign to our own DB
                var campaignEntityToCommit = new CampaignEntity
                {
                    SecondaryId = null,
                    CampaignGroup = 2,
                    Name = "Dummy campaign name",
                    BrandingText = "Dummy campaign branding text",
                    LocationInclude = new int[] { 0 },
                    LocationExclude = new int[] { 0 },
                    Language = "NL",
                    InitialCpc = 0.01M,
                    Budget = 1,
                    DailyBudget = null,
                    Delivery = Maximiz.Model.Enums.Delivery.Balanced,
                    StartDate = DateTime.Now,
                    Details = "{}"
                };
                await CommitCampaignWriteGuid(campaignEntityToCommit, token);

                // Create the object in the taboola API
                var accounts = (await GetAllAccounts(token)).Items;
                var account = _mapperAccount.Convert(accounts.ToList().Where(x => x.PartnerTypes.Contains("ADVERTISER")).First());
                CreateOrUpdateObjectsContext context = new CreateOrUpdateObjectsContext(1, null)
                {
                    Action = Maximiz.Model.CrudAction.Create,
                    Entity = new Entity[] { account, campaignEntityToCommit }
                };
                await CreateOrUpdateObjectsAsync(context, token);

                // Validate
                var created = context.Entity[1] as CampaignEntity;
                var campaignFromTaboola = await GetCampaign(account, created.SecondaryId, token);
                var campaignConverted = _mapperCampaign.Convert(campaignFromTaboola, created.Id);
                var campaignFromLocal = await FetchCampaignFromGuidAsync(created.Id, token);

                // Update
                campaignConverted.BrandingText = "Updated dummy branding text";
                context = new CreateOrUpdateObjectsContext(1, null)
                {
                    Action = Maximiz.Model.CrudAction.Update,
                    Entity = new Entity[] { account, campaignConverted }
                };
                await CreateOrUpdateObjectsAsync(context, token);

                // Validate
                var result2 = await GetCampaign(account, campaignEntityToCommit.SecondaryId, token);
                var resultConverted2 = _mapperCampaign.Convert(result2);
            }
            catch (Exception e)
            {
                _logger.LogError("Exception while testing: " + e.ToString());
            }
        }

    }
}
