using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poller.Database;
using Poller.Poller;
using Poller.Taboola;
using Poller.Taboola.Traffic;
using System;
using System.Threading;

using AccountInternal = Maximiz.Model.Entity.Account;
using CampaignInternal = Maximiz.Model.Entity.Campaign;
using CampaignExternal = Poller.Taboola.Model.Campaign;
using AdItemInternal = Maximiz.Model.Entity.AdItem;
using AdItemExternal = Poller.Taboola.Model.AdItemExternal;
using AdItemReports = Poller.Taboola.Model.AdItemReports;
using Poller.OAuth;
using Poller.Test.Taboola.Utility;
using System.Threading.Tasks;
using Maximiz.Helper;
using System.Linq;
using Maximiz.Model.Entity;
using Poller.Taboola.Mapper;
using Abp.Threading;
using Maximiz.Model;
using System.Net.Http;

namespace Poller.Test.Taboola.Integrated
{

    /// <summary>
    /// Tests the integration of our software with our own database and the
    /// Taboola API. This tests the CRUD operations interface, which is located
    /// at <see cref="CreateOrUpdateObjectsContext"/>.
    /// </summary>
    [TestClass]
    public class CrudTest : PollerBase
    {

        /// <summary>
        /// This creates all required object to perform poller executions.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            // First call base setup
            Setup();

            // Then remove all dummies
            AsyncHelper.RunSync(() => RemoveAllDebugFromDatabase());
        }

        /// <summary>
        /// Creates, updates and deletes a campaign entity across our entire system.
        /// </summary>
        [TestMethod]
        public async Task CrudInterfaceCampaign()
        {
            // Create our entity in our local database, get account.
            var campaignToCommit = await CreateCommitAndWriteGuidCampaign();

            // Create campaign
            var createdCampaign = await DriveCampaignCrudOperation(CrudAction.Create, campaignToCommit);

            // Update campaign
            createdCampaign.BrandingText = _updatedBrandingText;
            var updatedCampaign = await DriveCampaignCrudOperation(CrudAction.Update, createdCampaign);

            // Delete campaign
            var deletedCampaign = await DriveCampaignCrudOperation(CrudAction.Delete, updatedCampaign);
        }

        /// <summary>
        /// Creates a campaign entity to create, update and delete an ad item
        /// for this campaign. The campaign gets deleted at the end.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CrudInterfaceAdItem()
        {
            // Create our entity in our local database, get account.
            var campaignToCommit = await CreateCommitAndWriteGuidCampaign();
            var adItemToCommit = await CreateCommitAndWriteGuidAdItem(campaignToCommit);

            // Create the campaign
            var createdCampaign = await DriveCampaignCrudOperation(CrudAction.Create, campaignToCommit);

            // Create an ad item
            var createdAdItem = await DriveAdItemCrudOperation(CrudAction.Create, createdCampaign.SecondaryId, adItemToCommit);
            var updatedAdItem = await DriveAdItemCrudOperation(CrudAction.Update, createdCampaign.SecondaryId, adItemToCommit);
            var deletedAdItem = await DriveAdItemCrudOperation(CrudAction.Delete, createdCampaign.SecondaryId, adItemToCommit);

            // Delete the campaign
            var deletedCampaign = await DriveCampaignCrudOperation(CrudAction.Delete, createdCampaign);
        }

        /// <summary>
        /// Simulates a campaign syncback.
        /// </summary>
        /// <returns>Task</returns>
        [TestMethod]
        public async Task CrudInterfaceSyncback()
        {
            // Create our entity in our local database and externally
            var campaignToCommit = await CreateCommitAndWriteGuidCampaign(); 
            var createdCampaign = await DriveCampaignCrudOperation(CrudAction.Create, campaignToCommit);

            // Change something and update ONLY externally
            createdCampaign.BrandingText = _updatedBrandingText;
            await _crudExternal.UpdateCampaignAsync(_account, createdCampaign, _source.Token);

            // Drive CRUD syncback
            var campaignFromLocal = await _crudInternal.GetCampaignFromGuidAsync(createdCampaign.Id, _source.Token);
            var context = new CreateOrUpdateObjectsContext(0, null)
            {
                Action = CrudAction.Syncback,
                Entity = new Entity[] { _account, campaignFromLocal }
            };
            await _poller.CreateOrUpdateObjectsAsync(context, _source.Token);

            // Verify our syncback
            var campaignAfterSyncback = await _crudInternal.GetCampaignFromGuidAsync(createdCampaign.Id, _source.Token);
            Assert.AreEqual(campaignAfterSyncback.BrandingText, _updatedBrandingText);
        }

        /// <summary>
        /// Executes a CRUD context. This is the equivalent of performing these
        /// operations through the service bus, but without the service bus.
        /// </summary>
        /// <param name="action">The crud action</param>
        /// <param name="entity">The entity to crud</param>
        /// <returns>The affected campaign as retreived from our database</returns>
        private async Task<CampaignInternal> DriveCampaignCrudOperation(
            CrudAction action, Entity entity)
        {
            // Execute the context
            var context = CreateContext(action, entity);
            await _poller.CreateOrUpdateObjectsAsync(context, _source.Token);

            // Retreive and assert
            var campaignGuid = (context.Entity[1] as CampaignInternal).Id;
            var campaignIdExternal = (context.Entity[1] as CampaignInternal).SecondaryId;
            var campaignInternal = await _crudInternal.GetCampaignFromGuidAsync(campaignGuid, _source.Token);
            var campaignExternal = null as CampaignInternal;
            try
            {
                campaignExternal = await _crudExternal.GetCampaignAsync(
                    _account, campaignIdExternal, _source.Token);
            }
            // This returns 404 if we deleted the entity
            catch (HttpRequestException e) { if (context.Action != CrudAction.Delete) { throw e; } }

            // Assert
            if (context.Action == CrudAction.Delete)
            {
                Assert.IsNull(campaignInternal);
                Assert.IsNull(campaignExternal);
            }
            else
            {
                Assert.IsNotNull(campaignInternal.SecondaryId);
                Assert.IsNotNull(campaignExternal);
                Assert.IsNotNull(campaignExternal.Id);
                new Mappers.MapperCampaignTest().AssertOverlappingProperties(
                    campaignInternal, new MapperCampaign().Convert(campaignExternal));
            }

            // We are only interested in our internal object
            return campaignInternal;
        }

        /// <summary>
        /// Executes a CRUD context. This is the equivalent of performing these
        /// operations through the service bus, but without the service bus.
        /// </summary>
        /// <param name="action">The crud action</param>
        /// <param name="campaignIdExternal">The id of the corresponding campaign</param>
        /// <param name="entity">The entity to crud</param>
        /// <returns>The affected campaign as retreived from our database</returns>
        private async Task<AdItemInternal> DriveAdItemCrudOperation(
            CrudAction action, string campaignIdExternal, Entity entity)
        {
            // Execute the context
            var context = CreateContext(action, entity);
            await _poller.CreateOrUpdateObjectsAsync(context, _source.Token);

            // Retreive and assert
            var adItemGuid = (context.Entity[1] as AdItemInternal).Id;
            var adItemIdExternal = (context.Entity[1] as AdItemInternal).SecondaryId;
            var adItemInternal = await _crudInternal.GetAdItemFromGuidAsync(adItemGuid, _source.Token);
            var adItemExternal = null as AdItemInternal;
            try
            {
                adItemExternal = await _crudExternal.GetAdItemMainAsync(
                     _account, campaignIdExternal, adItemIdExternal, _source.Token);
            }
            // This returns 404 if we deleted the entity
            catch (HttpRequestException e) { if (context.Action != CrudAction.Delete) { throw e; } }

            // Assert
            if (context.Action == CrudAction.Delete)
            {
                Assert.IsNull(adItemInternal);
                Assert.IsNull(adItemExternal);
            }
            else
            {
                Assert.IsNotNull(adItemInternal.SecondaryId);
                Assert.IsNotNull(adItemExternal);
                new Mappers.MapperAdItemTest().AssertOverlappingPropertiesMain(
                    adItemInternal, new MapperAdItem().Convert(adItemExternal));
            }

            // We are only interested in our internal object
            return adItemInternal;
        }

        /// <summary>
        /// Creates a campaign entity in our database, writes the GUID to it
        /// and returns the result.
        /// </summary>
        /// <returns>The created campaign with GUID</returns>
        private async Task<CampaignInternal> CreateCommitAndWriteGuidCampaign()
        {
            var result = new BareMinimumModels().CreateBareMinimumCampaign(
                name: _campaignName,
                campaignGroup: null,
                brandingText: "Testing branding text");

            result = await _crudUtility.CommitCampaignWriteGuid(result, _source.Token);
            Assert.IsNotNull(result.Id);
            Assert.IsNull(result.SecondaryId);

            return result;
        }

        /// <summary>
        /// Creates an ad item in our internal database as if the backend did.
        /// </summary>
        /// <param name="campaign">The campaign to which the ad item belongs</param>
        /// <returns>The internally created ad item</returns>
        private async Task<AdItemInternal> CreateCommitAndWriteGuidAdItem(CampaignInternal campaign)
        {
            var result = new BareMinimumModels().CreateBareMinimumAdItem(
                campaign: campaign,
                adGroup: null,
                adGroupImageIndex: -1,
                adGroupTitleIndex: -1,
                title: _adItemTitle,
                url: _url);

            result = await _crudUtility.CommitAdItemWriteGuid(result, _source.Token);
            Assert.IsNotNull(result.Id);
            Assert.IsNull(result.SecondaryId);

            return result;
        }

        /// <summary>
        /// Constructs a context for us.
        /// </summary>
        /// <param name="crudAction">What to do with the entity</param>
        /// <param name="entity">The respective entity</param>
        /// <returns>The created context</returns>
        private CreateOrUpdateObjectsContext CreateContext(CrudAction crudAction, Entity entity)
        {
            return new CreateOrUpdateObjectsContext(1, null)
            {
                Action = crudAction,
                Entity = new Entity[] { _account, entity }
            };
        }

        /// <summary>
        /// Removes all the mess that we have created.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            // First remove all dummies
            AsyncHelper.RunSync(() => RemoveAllDebugFromDatabase());

            // Call base last
            CleanUp();
        }

        /// <summary>
        /// Removes all dummy campaigns and ad items from our database.
        /// Also removes all dummy campaigns from Taboola.
        /// </summary>
        /// <returns>Task</returns>
        private async Task RemoveAllDebugFromDatabase()
        {
            await _crudUtility.RemoveInternalAllDummyAdItemsAsync(_adItemTitle);
            await _crudUtility.RemoveExternalAllDummyCampaignsAsync(_campaignName);
            await _crudUtility.RemoveInternalAllDummyCampaignsAsync(_campaignName);
        }

    }
}
