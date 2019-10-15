using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poller.Poller;
using System;
using Poller.Extensions;

using AccountInternal = Maximiz.Model.Entity.Account;
using CampaignInternal = Maximiz.Model.Entity.Campaign;
using AdItemInternal = Maximiz.Model.Entity.AdItem;
using System.Threading.Tasks;
using System.Linq;
using Poller.Taboola.Mapper;
using System.Collections.Generic;
using Poller.Test.Taboola.Mappers;

namespace Poller.Test.Taboola.Integrated
{

    /// <summary>
    /// Tests the integration of our software with our own database and the
    /// Taboola API, for the <see cref="IPollerDataSyncback"/> interface.
    /// 
    /// TODO Initialize and cleanup run before and after each test. This means
    /// some objects are disposed after they are created a second time.
    /// </summary>
    [TestClass]
    public class SyncbackTest : PollerBase
    {

        /// <summary>
        /// This creates all required object to perform poller executions.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            Setup();
        }

        /// <summary>
        /// Simulates a syncback operation.
        /// </summary>
        [TestMethod]
        public async Task DriveSyncbackWithAccounts()
        {
            // This is where the created items will also be stored
            var accounts = new List<AccountInternal>();
            var campaigns = new List<CampaignInternal>();
            var adItems = new List<AdItemInternal>();

            var campaignsSelected = new List<CampaignInternal>();
            var adItemsSelected = new List<AdItemInternal>();

            // ((runCount + 1) % 30 == 0) --> also syncback accounts
            var context = new PollerContext(runCount: 29, lastRun: null);
            await _poller.DataSyncbackAsync(context, _source.Token, accounts, campaigns, adItems);

            // Assert some random items to exist both internal and external
            campaignsSelected = campaigns.Shuffle().Take(2).ToList();
            adItemsSelected = adItems.Shuffle().Take(4).ToList();

            foreach (var campaign in campaignsSelected) { await AssertExistsCampaign(campaign); }
            foreach (var adItem in adItemsSelected) { await AssertExistsAdItem(adItem); }
        }

        /// <summary>
        /// Asserts that some campaign exists both internally and externally. Also
        /// compares its properties.
        /// </summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>Task</returns>
        private async Task AssertExistsCampaign(CampaignInternal campaign)
        {
            // Get
            var accountId = new MapperCampaign().Convert(campaign).Account;
            var accounts = await _crudInternal.GetAdvertiserAccountsCachedAsync(_source.Token);
            var accountSpecific = accounts.Where(x => x.Name.Equals(accountId)).FirstOrDefault();
            campaign.Id = (await _crudInternal.GetCampaignFromExternalIdAsync(campaign.SecondaryId, _source.Token)).Id;

            var campaignInternal = await _crudInternal.GetCampaignFromGuidAsync(campaign.Id, _source.Token);
            var campaignExternal = await _crudExternal.GetCampaignAsync(accountSpecific, campaign.SecondaryId, _source.Token);

            // Assert
            Assert.IsNotNull(campaignInternal);
            Assert.IsNotNull(campaignExternal);
            new MapperCampaignTest().AssertOverlappingProperties(campaignInternal,
                new MapperCampaign().Convert(campaignExternal));
        }

        /// <summary>
        /// Asserts that some ad item exists both internally and externally.
        /// Also compares its properties.
        /// </summary>
        /// <param name="adItem">The ad item</param>
        /// <returns>Task</returns>
        private async Task AssertExistsAdItem(AdItemInternal adItem)
        {
            // Get
            var campaignId = new MapperAdItem().Convert(adItem).CampaignId;
            var campaignSpecific = await _crudInternal.GetCampaignFromExternalIdAsync(campaignId, _source.Token);
            var accounts = await _crudInternal.GetAdvertiserAccountsCachedAsync(_source.Token);
            var accountId = new MapperCampaign().Convert(campaignSpecific).Account;
            var accountSpecific = accounts.Where(x => x.Name.Equals(accountId)).FirstOrDefault();

            var adItemInternal = await _crudInternal.GetAdItemFromExternalIdAsync(
                adItem.SecondaryId, _source.Token);
            var adItemExternal = await _crudExternal.GetAdItemMainAsync(
                accountSpecific, campaignSpecific.SecondaryId, adItem.SecondaryId, _source.Token);

            // Assert
            Assert.IsNotNull(adItemInternal);
            Assert.IsNotNull(adItemExternal);
            new MapperAdItemTest().AssertOverlappingPropertiesMain(
                adItemInternal, new MapperAdItem().Convert(adItemExternal));
        }

        /// <summary>
        /// Removes all the mess that we have created.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            CleanUp();
        }

    }
}
