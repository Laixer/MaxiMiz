using System;
using Poller.Taboola.Mapper;
using Maximiz.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maximiz.Model.Enums;
using Poller.Taboola.Model;
using Poller.Model.Data;
using Poller.Helper;

using CampaignInternal = Maximiz.Model.Entity.Campaign;
using CampaignExternal = Poller.Taboola.Model.Campaign;
using ApprovalStateInternal = Maximiz.Model.Enums.ApprovalState;
using ApprovalStateExternal = Poller.Taboola.Model.ApprovalState;
using CampaignStatusExternal = Poller.Taboola.Model.CampaignStatus;
using CampaignStatusInternal = Maximiz.Model.Enums.CampaignStatus;

namespace Poller.Test.Taboola
{

    /// <summary>
    /// Used for testing our <see cref="MapperCampaign"/>.
    /// TODO Many edge cases
    /// TODO Ignoring details at conversion back to Taboola.
    /// TODO Targets
    /// </summary>
    [TestClass]
    public class MapperCampaignTest
    {

        /// <summary>
        /// Campaign mapper.
        /// </summary>
        private MapperCampaign _mapperCampaign;

        /// <summary>
        /// Creates Taboola bare minimals for us.
        /// </summary>
        private BareMinimumTaboola _bareMinimumExternal;

        /// <summary>
        /// Creates internal bare minimals for us.
        /// </summary>
        private BareMinimumModels _bareMinimumInternal;

        /// <summary>
        /// Contains conversion utility functions for us.
        /// </summary>
        private MapperUtility _utility;

        private const string externalId = "13371337";
        private static readonly Guid internalId = Guid.NewGuid();
        private const string utm = "my-tracking-code";
        private const decimal dailyBudget = 25;
        private const string note = "This is my note";
        private const decimal spent = 1337;
        private static readonly DateTime? startDate = DateTime.Now;
        private static readonly DateTime? endDate = DateTime.Now;
        private const string name = "Used campaign name";
        private const string brandingText = "Used branding text";
        private const decimal cpc = 11;
        private const decimal spendingLimit = 100;
        private const string accountId = "12345678";
        private const decimal cpaGoal = 3;
        private const BidStrategy bidStrategy = BidStrategy.Smart;
        private const BidType bidType = BidType.OptimizedConversions;
        private const BudgetModel budgetModel = BudgetModel.Monthly;
        private const SpendingLimitModel spendingLimitModel = SpendingLimitModel.Monthly;
        private const ApprovalStateInternal approvalStateInternal = ApprovalStateInternal.Approved;
        private const ApprovalStateExternal approvalStateExternal = ApprovalStateExternal.Approved;
        private const CampaignStatusExternal statusExternal = CampaignStatusExternal.Running;
        private const CampaignStatusInternal statusInternal = CampaignStatusInternal.Running;
        private static readonly PublisherBidModifier publisherBidModifier = new PublisherBidModifier { Values = null };
        private const DailyAdDeliveryModel dailyAdDeliveryModel = DailyAdDeliveryModel.Balanced;
        private const TrafficAllocationMode trafficAllocationMode = TrafficAllocationMode.Even;
        private const bool active = true;
        private const MarketingObjective marketingObjective = MarketingObjective.LeadsGeneration;
        private const string targetUrl = "mytargeturl.com";
        private static readonly Guid campaignGroupGuid = Guid.NewGuid();
        private const Maximiz.Model.Enums.Publisher publisher = Maximiz.Model.Enums.Publisher.Taboola;
        private const string language = "AB";
        private const Delivery delivery = Delivery.Strict;

        /// <summary>
        /// Sets up our objects for testing.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _bareMinimumExternal = new BareMinimumTaboola();
            _bareMinimumInternal = new BareMinimumModels();
            _utility = new MapperUtility();
            _mapperCampaign = new MapperCampaign();
        }

        /// <summary>
        /// Converts from Taboola to internal to Taboola.
        /// </summary>
        [TestMethod]
        public void ExternalToInternalToExternal()
        {
            // Create and convert
            var campaignExternal = CreateCampaignExternal();
            var campaignInternal = _mapperCampaign.Convert(campaignExternal);
            var campaignExternalReconverted = _mapperCampaign.Convert(campaignInternal);

            // Assert
            AssertOverlappingProperties(campaignInternal, campaignExternal);
            AssertOverlappingProperties(campaignInternal, campaignExternalReconverted);
            AssertDetails(campaignInternal);
        }

        /// <summary>
        /// Converts from internal to Taboola to internal.
        /// </summary>
        [TestMethod]
        public void InternalToExternalToInternal()
        {
            var campaignInternal = CreateCampaignInternal();
            var campaignExternal = _mapperCampaign.Convert(campaignInternal);
            var campaignInternalConverted = _mapperCampaign.Convert(campaignExternal, campaignInternal.Id, campaignInternal.CampaignGroupGuid);

            // Assert
            AssertDetails(campaignInternal);
            AssertDetails(campaignInternalConverted);
            AssertOverlappingProperties(campaignInternal, campaignExternal);
            AssertOverlappingProperties(campaignInternalConverted, campaignExternal);

            // Assert specifics
            Assert.AreEqual(campaignInternal.Id, campaignInternalConverted.Id);
            Assert.AreEqual(campaignInternal.CampaignGroupGuid, campaignInternalConverted.CampaignGroupGuid);
        }

        /// <summary>
        /// Validates all our internal properties between both the internal and
        /// external object.
        /// </summary>
        /// <param name="campaignInternal">The internal campaign</param>
        /// <param name="campaignExternal">The external campaign</param>
        [TestMethod]
        private void AssertOverlappingProperties(CampaignInternal campaignInternal,
            CampaignExternal campaignExternal)
        {
            // Assert regular variables
            Assert.AreEqual(campaignInternal.SecondaryId, campaignExternal.Id);
            Assert.AreEqual(campaignInternal.Name, campaignExternal.Name);
            Assert.AreEqual(campaignInternal.BrandingText, campaignExternal.BrandingText);
            Assert.AreEqual(campaignInternal.Utm, campaignExternal.Utm);
            Assert.AreEqual(campaignInternal.InitialCpc, campaignExternal.Cpc);
            Assert.AreEqual(campaignInternal.DailyBudget, campaignExternal.DailyCap);
            Assert.AreEqual(campaignInternal.Budget, campaignExternal.SpendingLimit);
            Assert.AreEqual(campaignInternal.Note, campaignExternal.Note);
            Assert.AreEqual(campaignInternal.Spent, campaignExternal.Spent);
            Assert.AreEqual(campaignInternal.StartDate, campaignExternal.StartDate);
            Assert.AreEqual(campaignInternal.EndDate, campaignExternal.EndDate);

            // Assert enums after extra conversion
            Assert.AreEqual(campaignInternal.BidStrategy,
                _mapperCampaign.BidStrategyToInternal(campaignExternal.BidStrategy));
            Assert.AreEqual(campaignInternal.ApprovalState,
                _utility.ApprovalStateToInternal(campaignExternal.ApprovalState));
            Assert.AreEqual(campaignInternal.Status,
                _mapperCampaign.CampaignStatusToInternal(campaignExternal.Status));
            Assert.AreEqual(campaignInternal.Delivery,
                _mapperCampaign.DeliveryToInternal(campaignExternal.DailyAdDeliveryModel));
        }

        /// <summary>
        /// This verifies our serialized details to be correct.
        /// </summary>
        /// <param name="campaignInternal">The internal object</param>
        [TestMethod]
        private void AssertDetails(CampaignInternal campaignInternal)
        {
            var extracted = Json.Deserialize<CampaignDetails>(campaignInternal.Details);

            Assert.AreEqual(extracted.Account, accountId);
            Assert.AreEqual(extracted.PublisherBidModifier.Values, publisherBidModifier.Values);
            Assert.AreEqual(extracted.CpaGoal, cpaGoal);
            Assert.AreEqual(extracted.TrafficAllocationMode, trafficAllocationMode);
            Assert.AreEqual(extracted.Active, active);
            Assert.AreEqual(extracted.MarketingObjective, marketingObjective);
        }

        /// <summary>
        /// Creates an external campaign for us.
        /// </summary>
        /// <returns>The new campaign</returns>
        private CampaignExternal CreateCampaignExternal()
        {
            var campaignExternal = _bareMinimumExternal.CreateBareMinimumCampaign(
                name: name,
                brandingText: brandingText,
                cpc: cpc,
                spendingLimit: spendingLimit);
            campaignExternal.Id = externalId;
            campaignExternal.Utm = utm;
            campaignExternal.DailyCap = dailyBudget;
            campaignExternal.BidStrategy = _utility.ToUpperString(bidStrategy);
            campaignExternal.SpendingLimitModel = _utility.ToUpperString(spendingLimitModel);
            campaignExternal.Note = note;
            campaignExternal.Spent = spent;
            campaignExternal.StartDate = startDate;
            campaignExternal.EndDate = endDate;
            campaignExternal.ApprovalState = _utility.ToUpperString(approvalStateExternal);
            campaignExternal.Status = _utility.ToUpperString(statusExternal);
            campaignExternal.PublisherBidModifier = publisherBidModifier;
            campaignExternal.Account = accountId;
            campaignExternal.CpaGoal = cpaGoal;
            campaignExternal.DailyAdDeliveryModel = _utility.ToUpperString(dailyAdDeliveryModel);
            campaignExternal.TrafficAllocationMode = _utility.ToUpperString(trafficAllocationMode);
            campaignExternal.Active = active;
            campaignExternal.MarketingObjective = _utility.ToUpperString(marketingObjective);

            return campaignExternal;
        }

        /// <summary>
        /// Creates an internal campaign for us.
        /// </summary>
        /// <returns>The created campaign<returns>
        private CampaignInternal CreateCampaignInternal()
        {
            var result = _bareMinimumInternal.CreateBareMinimumCampaign(
                name: name,
                campaignGroup: null,
                brandingText: brandingText,
                language: language,
                inititalCpc: cpc,
                budget: spendingLimit,
                delivery: delivery,
                bidStrategy: bidStrategy,
                budgetModel: budgetModel);

            result.Id = internalId;
            result.SecondaryId = externalId;
            result.CampaignGroupGuid = campaignGroupGuid;
            result.Utm = utm;
            result.DailyBudget = dailyBudget;
            result.Budget = spendingLimit;
            result.Note = note;
            result.Spent = spent;
            result.StartDate = startDate;
            result.EndDate = endDate;
            result.ApprovalState = approvalStateInternal;
            result.Status = statusInternal;

            var details = CreateDetails();
            result.Details = Json.Serialize(details);

            // TODO Location include exclude thingy
            // Plus targeting

            return result;
        }

        /// <summary>
        /// Generates campaign details for us.
        /// </summary>
        /// <returns>The new campaign details object</returns>
        private CampaignDetails CreateDetails()
        {
            return new CampaignDetails
            {
                PublisherBidModifier = publisherBidModifier,
                Account = accountId,
                CpaGoal = cpaGoal,
                TrafficAllocationMode = trafficAllocationMode,
                Active = active,
                MarketingObjective = marketingObjective
            };
        }
    }
}
