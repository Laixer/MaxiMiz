using System;
using System.Collections.Generic;
using System.Linq;
using Maximiz.Helper;
using Poller.Helper;
using Poller.Model.Data;
using Poller.Taboola.Model;
using CampaignInternal = Maximiz.Model.Entity.Campaign;
using CampaignExternal = Poller.Taboola.Model.Campaign;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Our converter for campaigns. This section of the partial class contains
    /// all main conversion functions.
    /// TODO Update for new entity model.
    /// </summary>
    internal partial class MapperCampaign : IMapper<CampaignExternal, CampaignInternal>
    {

        /// <summary>
        /// Mapper for our target objects and relevant fields.
        /// </summary>
        private MapperTarget _mapperTarget { get; set; }

        /// <summary>
        /// Generates bare minimum internal entities.
        /// </summary>
        private BareMinimumModels _bareMinimumInternal { get; set; }

        /// <summary>
        /// Generates bare minimum external entities.
        /// </summary>
        private BareMinimumTaboola _bareMinimumExternal { get; set; }

        /// <summary>
        /// Used for utility functions.
        /// </summary>
        private MapperUtility _utility { get; set; }

        /// <summary>
        /// Constructor to create instances.
        /// </summary>
        public MapperCampaign()
        {
            _utility = new MapperUtility();
            _bareMinimumInternal = new BareMinimumModels();
            _bareMinimumExternal = new BareMinimumTaboola();
            _mapperTarget = new MapperTarget();
        }

        /// <summary>
        /// Converts our core model to taboola campaign. This preserves the GUID.
        /// </summary>
        /// <param name="core">The object to convert</param>
        /// <param name="guid">The guid if we already know it</param>
        /// <param name="campaignGroupGuid">The campaign group guid</param>
        /// <returns>The converted object</returns>
        public CampaignInternal Convert(CampaignExternal core, Guid guid, Guid? campaignGroupGuid = null)
        {
            var converted = Convert(core);
            converted.Id = guid;
            if (campaignGroupGuid != null && campaignGroupGuid != Guid.Empty)
            {
                converted.CampaignGroupGuid = (Guid)campaignGroupGuid;
            }
            return converted;
        }

        /// <summary>
        /// Converts our core model to taboola campaign.
        /// </summary>
        /// <param name="campaignInternal">The object to convert</param>
        /// <returns>The converted object</returns>
        public CampaignExternal Convert(CampaignInternal campaignInternal)
        {
            if (campaignInternal == null) throw new ArgumentNullException(nameof(campaignInternal));

            // Create a base
            var campaignExternal = _bareMinimumExternal.CreateBareMinimumCampaign();

            // Append all
            campaignExternal.Id = campaignInternal.SecondaryId;
            campaignExternal.Name = campaignInternal.Name;
            campaignExternal.BrandingText = campaignInternal.BrandingText;
            campaignExternal.Utm = campaignInternal.Utm;
            campaignExternal.Cpc = campaignInternal.InitialCpc;
            campaignExternal.SpendingLimit = campaignInternal.Budget;
            campaignExternal.SpendingLimitModel = BudgetModelToString(campaignInternal.BudgetModel);
            campaignExternal.Note = campaignInternal.Note;
            campaignExternal.Spent = campaignInternal.Spent;
            campaignExternal.BidStrategy = BidStrategyToString(campaignInternal.BidStrategy);
            campaignExternal.DailyCap = campaignInternal.BudgetDaily;
            campaignExternal.StartDate = campaignInternal.StartDate;
            campaignExternal.EndDate = campaignInternal.EndDate;
            campaignExternal.ApprovalState = _utility.ApprovalStateToString(campaignInternal.ApprovalState);
            campaignExternal.Status = CampaignStatusToString(campaignInternal.Status);
            campaignExternal.DailyAdDeliveryModel = DeliveryToString(campaignInternal.Delivery);

            // Append details
            PushDetails(campaignExternal, campaignInternal.Details);

            // Map targets
            _mapperTarget.MapAllTargeting(campaignExternal, campaignInternal);

            return campaignExternal;
        }

        /// <summary>
        /// Pushes our details to a given taboola campaign object. This will 
        /// overwrite in all cases.
        /// </summary>
        /// <remarks>This does nothing if the details string is empty or null</remarks>
        /// <param name="to">The object to push to</param>
        /// <param name="detailsJson">The json formatted details string</param>
        /// <return>The pushed object with extracted details</return>
        private CampaignExternal PushDetails(CampaignExternal to, string detailsJson)
        {
            if (string.IsNullOrEmpty(detailsJson)) { return to; }
            var details = Json.Deserialize<CampaignDetails>(detailsJson);

            if (to == null) throw new ArgumentNullException(nameof(to));
            if (details == null) throw new ArgumentNullException(nameof(details));

            to.Account = details.Account;
            to.PublisherBidModifier = details.PublisherBidModifier;
            to.CountryTargeting = details.CountryTargeting;
            to.SubCountryTargeting = details.SubCountryTargeting;
            to.PostalCodeTargeting = details.PostalCodeTargeting;
            to.ContextualTargeting = details.ContextualTargeting;
            to.PlatformTargeting = details.PlatformTargeting;
            to.OsTargeting = details.OsTargeting;
            to.ConnectionTypeTargeting = details.ConnectionTypeTargeting;
            to.CpaGoal = details.CpaGoal;
            to.TrafficAllocationMode = _utility.ToUpperString(details.TrafficAllocationMode);
            to.Active = details.Active;
            to.MarketingObjective = _utility.ToUpperString(details.MarketingObjective);

            return to;
        }

        /// <summary>
        /// Converts a taboola campaign to our core model.
        /// </summary>
        /// <param name="external">The object to convert</param>
        /// <returns>The converted object</returns>
        public CampaignInternal Convert(CampaignExternal external)
        {
            if (external == null) throw new ArgumentNullException(nameof(external));

            // Create a base
            var result = _bareMinimumInternal.CreateBareMinimumCampaign(
                name: external.Name,
                campaignGroup: null);

            // Append everything
            result.SecondaryId = external.Id;
            result.Name = external.Name;
            result.BrandingText = external.BrandingText;
            result.Utm = external.Utm;
            result.InitialCpc = external.Cpc;
            result.BudgetDaily = external.DailyCap;
            result.BidStrategy = BidStrategyToInternal(external.BidStrategy);
            result.Budget = external.SpendingLimit;
            result.BudgetModel = BudgetModelToInternal(external.SpendingLimitModel);
            result.Note = external.Note;
            result.Spent = external.Spent;
            result.StartDate = _utility.ConvertTaboolaDateTime(external.StartDate);
            result.EndDate = _utility.ConvertTaboolaDateTime(external.EndDate);
            result.ApprovalState = _utility.ApprovalStateToInternal(external.ApprovalState);
            result.Status = CampaignStatusToInternal(external.Status);
            result.Delivery = DeliveryToInternal(external.DailyAdDeliveryModel);

            // Append details
            result.Details = ExtractDetailsToString(external);

            // Sanetize edge cases.
            // End date of 12-31-9999 means null.
            // Daily cap of 0 means unlimited, which we denote as null.
            if (external.EndDate.Equals(new DateTime(9999, 12, 31))) { result.EndDate = null; }
            if (result.BudgetDaily == 0) { result.BudgetDaily = null; }

            return result;
        }

        /// <summary>
        /// Extracts the details from a Taboola campaign.
        /// This then converts it to a JSON string.
        /// </summary>
        /// <param name="from">The Taboola campaign</param>
        /// <returns>The details object as JSON string</returns>
        private string ExtractDetailsToString(CampaignExternal from)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));

            return Json.Serialize(new CampaignDetails
            {
                Account = from.Account,
                PublisherBidModifier = from.PublisherBidModifier,
                CountryTargeting = from.CountryTargeting,
                SubCountryTargeting = from.SubCountryTargeting,
                PostalCodeTargeting = from.PostalCodeTargeting,
                ContextualTargeting = from.ContextualTargeting,
                PlatformTargeting = from.PlatformTargeting,
                OsTargeting = from.OsTargeting,
                ConnectionTypeTargeting = from.ConnectionTypeTargeting,
                CpaGoal = from.CpaGoal,
                TrafficAllocationMode = _utility.FromUpperString(from.TrafficAllocationMode, TrafficAllocationMode.Even),
                Active = from.Active,
                MarketingObjective = _utility.FromUpperString(from.MarketingObjective, MarketingObjective.DriveWebsiteTraffic)
            });
        }

        /// <summary>
        /// Bulk conversion from Taboola to Core.
        /// </summary>
        /// <param name="list">Taboola list</param>
        /// <returns>Core list</returns>
        public IEnumerable<CampaignInternal> ConvertAll(IEnumerable<CampaignExternal> list)
        {
            IList<CampaignInternal> result = new List<CampaignInternal>();
            foreach (var item in list.AsParallel())
            {
                result.Add(Convert(item));
            }
            return result;
        }

        /// <summary>
        /// Bulk conversion from Core to Taboola.
        /// </summary>
        /// <param name="list">Core list</param>
        /// <returns>Taboola list</returns>
        public IEnumerable<CampaignExternal> ConvertAll(IEnumerable<CampaignInternal> list)
        {
            IList<CampaignExternal> result = new List<CampaignExternal>();
            foreach (var item in list.AsParallel())
            {
                result.Add(Convert(item));
            }
            return result;
        }

    }
}
