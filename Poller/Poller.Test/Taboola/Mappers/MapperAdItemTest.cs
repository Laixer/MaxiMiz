using Maximiz.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poller.Taboola.Mapper;
using System;
using Poller.Helper;
using Poller.Taboola.Model;

using AdItemInternal = Maximiz.Model.Entity.AdItem;
using AdItemExternal = Poller.Taboola.Model.AdItemExternal;
using AdItemReports = Poller.Taboola.Model.AdItemReports;
using ApprovalStateInternal = Maximiz.Model.Enums.ApprovalState;
using ApprovalStateExternal = Poller.Taboola.Model.ApprovalState;
using AdItemStatusExternal = Poller.Taboola.Model.CampaignItemStatus;
using AdItemStatusInternal = Maximiz.Model.Enums.AdItemStatus;

namespace Poller.Test.Taboola.Mappers
{

    /// <summary>
    /// Tests our <see cref="MapperAdItem"/>.
    /// TODO Ignoring merge
    /// TODO Many edge cases
    /// TODO Ignoring details at conversion back to Taboola.
    /// </summary>
    [TestClass]
    public class MapperAdItemTest
    {

        // Used for both
        private static readonly Guid internalId = Guid.NewGuid();
        private const string externalId = "13371337";
        private const string url = "thisismytargetpage.com";
        private const string imageUrl = "myimage.com";
        private const string title = "Ad item title";
        private const decimal spent = 3000;
        private const int clicks = 1337;
        private const int impressions = 13370000;
        private const int actions = 48;
        private const decimal cpc = 11;
        private const AdItemStatusInternal adItemStatusInternal = AdItemStatusInternal.Running;
        private const AdItemStatusExternal adItemStatusExternal = AdItemStatusExternal.Running;
        private const ApprovalStateInternal approvalStateInternal = ApprovalStateInternal.Approved;
        private const ApprovalStateExternal approvalStateExternal = ApprovalStateExternal.Approved;

        // Used for our internal model
        private const string content = "This is my content. Don't touch my content. It's mine.";
        private static readonly Guid campaignGuid = Guid.NewGuid();
        private static readonly Guid adGroupGuid = Guid.NewGuid();
        private const int adGroupImageIndex = 2;
        private const int adGroupTitleIndex = 1;

        // Used for our external model (main AND reports)
        private const string campaignIdExternal = "13378865";

        // Used for our external model (main)
        private const bool active = true;
        private const string typeAsString = "ITEM";

        // Used for our external model (reports)
        private const string campaignName = "Campaign name";
        private const string contentProvider = "13388594";
        private const string contentProviderName = "My content provider";
        private const string currency = "EUR";

        /// <summary>
        /// Contains mapping utility functions.
        /// </summary>
        private MapperUtility _utility;

        /// <summary>
        /// Maps our ad items.
        /// </summary>
        private MapperAdItem _mapperAdItem;

        /// <summary>
        /// Creates internal minimal objects.
        /// </summary>
        private BareMinimumModels _bareMinimumInternal;

        /// <summary>
        /// Creates external minimal objects.
        /// </summary>
        private BareMinimumTaboola _bareMinimumExternal;

        /// <summary>
        /// Constructor to prevent nullpointers.
        /// TODO This is bad design. (is it?)
        /// </summary>
        public MapperAdItemTest()
        {
            Setup();
        }

        /// <summary>
        /// Sets up our objects before testing.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _utility = new MapperUtility();
            _mapperAdItem = new MapperAdItem();
            _bareMinimumInternal = new BareMinimumModels();
            _bareMinimumExternal = new BareMinimumTaboola();
        }

        /// <summary>
        /// Converts from external to internal to external.
        /// </summary>
        [TestMethod]
        public void ExternalInternalExternal()
        {
            // Create and convert
            var externalAdItem = CreateAdItemExternal();
            var internalAdItem = _mapperAdItem.Convert(externalAdItem);
            var externalAdItemConverted = _mapperAdItem.Convert(internalAdItem);

            // Assert
            AssertOverlappingPropertiesMain(internalAdItem, externalAdItem);
            AssertOverlappingPropertiesMain(internalAdItem, externalAdItemConverted);
            AssertDetailsMain(internalAdItem);
        }

        /// <summary>
        /// Converts from internal to external to internal.
        /// </summary>
        [TestMethod]
        public void InternalExternalInternal()
        {
            // Create and convert
            var internalAdItem = CreateAdItemInternal();
            var externalAdItem = _mapperAdItem.Convert(internalAdItem);
            var internalAdItemConverted = _mapperAdItem.Convert(externalAdItem, 
                internalAdItem.Id, internalAdItem.CampaignGuid);

            // Assert
            AssertOverlappingPropertiesMain(internalAdItem, externalAdItem);
            AssertOverlappingPropertiesMain(internalAdItemConverted, externalAdItem);
            AssertDetailsMain(internalAdItem);
            AssertDetailsMain(internalAdItemConverted); ;
        }

        /// <summary>
        /// Compares overlapping fields.
        /// </summary>
        /// <param name="adItemInternal">Internal item</param>
        /// <param name="adItemExternal">External item, main, not reports</param>
        internal void AssertOverlappingPropertiesMain(AdItemInternal adItemInternal,
            AdItemExternal adItemExternal)
        {
            // Regular values
            Assert.AreEqual(adItemInternal.SecondaryId, adItemExternal.Id);
            Assert.AreEqual(adItemInternal.Url, adItemExternal.Url);
            Assert.AreEqual(adItemInternal.ImageUrl, adItemExternal.ThumbnailUrl);
            Assert.AreEqual(adItemInternal.Title, adItemExternal.Title);

            // Converted enums
            Assert.AreEqual(adItemInternal.ApprovalState,
                _utility.ApprovalStateToInternal(adItemExternal.ApprovalState));
            Assert.AreEqual(adItemInternal.Status,
                _mapperAdItem.AdItemStatusToInternal(adItemExternal.AdItemStatus));
        }

        /// <summary>
        /// Asserts details created from an external main ad item.
        /// </summary>
        /// <param name="adItemInternal">The internal ad item containing
        /// the details in its json details field.</param>
        private void AssertDetailsMain(AdItemInternal adItemInternal)
        {
            var details = Json.Deserialize<AdItemDetails>(adItemInternal.Details);

            Assert.AreEqual(details.Active, active);
            Assert.AreEqual(details.Type, typeAsString);
            Assert.AreEqual(details.CampaignId, campaignIdExternal);
        }

        /// <summary>
        /// Asserts details created from an external main ad item.
        /// </summary>
        /// <param name="adItemInternal">The internal ad item containing
        /// the details in its json details field.</param>
        private void AssertDetailsReports(AdItemInternal adItemInternal)
        {
            var details = Json.Deserialize<AdItemDetails>(adItemInternal.Details);

            Assert.AreEqual(details.CampaignId, campaignIdExternal); 
            Assert.AreEqual(details.CampaignName, campaignName); 
            Assert.AreEqual(details.ContentProvider, contentProvider); 
            Assert.AreEqual(details.ContentProviderName, contentProviderName); 
            Assert.AreEqual(details.Currency, currency); 
        }

        /// <summary>
        /// Creates a new Taboola ad item (non report, but main) for us/
        /// </summary>
        /// <returns>The created object</returns>
        private AdItemExternal CreateAdItemExternal()
        {
            var result = _bareMinimumExternal.CreateBareMinimumAdItem(url: url);

            result.Id = campaignIdExternal;
            result.ThumbnailUrl = imageUrl;
            result.Title = title;
            result.ApprovalState = _utility.ToUpperString(approvalStateExternal);
            result.AdItemStatus = _utility.ToUpperString(adItemStatusExternal);
            result.Active = active;
            result.Type = typeAsString;
            result.CampaignId = campaignIdExternal;

            return result;
        }

        /// <summary>
        /// Creates a report item for us.
        /// </summary>
        /// <returns>The created object</returns>
        private AdItemReports CreateAdItemReports()
        {
            return new AdItemReports
            {
                Id = externalId,
                Url = url,
                ThumbnailUrl = imageUrl,
                Title = title,
                Spent = spent,
                Clicks = clicks,
                Impressions = impressions,
                Actions = actions,
                Cpc = cpc,
                Campaign = campaignIdExternal,
                CampaignName = campaignName,
                ContentProvider = contentProvider,
                ContentProviderName = contentProviderName,
                Currency = currency
            };
        }

        /// <summary>
        /// Creates an internal ad item.
        /// </summary>
        /// <returns>The new object</returns>
        private AdItemInternal CreateAdItemInternal()
        {
            var result = _bareMinimumInternal.CreateBareMinimumAdItem(
                null,
                null,
                adGroupImageIndex: adGroupImageIndex,
                adGroupTitleIndex: adGroupTitleIndex,
                title: title,
                url: url);

            result.Id = internalId;
            result.ApprovalState = approvalStateInternal;
            result.Status = adItemStatusInternal;
            result.Spent = spent;
            result.Clicks = clicks;
            result.Impressions = impressions;
            result.Actions = actions;
            result.Cpc = cpc;
            result.Details = Json.Serialize(CreateAdItemDetails());
            result.Content = content;
            result.CampaignGuid = campaignGuid;
            result.ModifiedBeyondAdGroup = true;

            return result;
        }

        /// <summary>
        /// Creates an ad item details object.
        /// </summary>
        /// <returns>The created object</returns>
        private AdItemDetails CreateAdItemDetails()
        {
            return new AdItemDetails
            {
                Active = active,
                Type = typeAsString,
                CampaignId = campaignIdExternal,
                CampaignName = campaignName,
                ContentProvider = contentProvider,
                ContentProviderName = contentProviderName,
                Currency = currency
            };
        }

    }
}
