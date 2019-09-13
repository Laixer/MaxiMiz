using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Maximiz.Model.Enums;
using Poller.Helper;
using Poller.Model.Data;
using Poller.Taboola.Model;
using CampaignCore = Maximiz.Model.Entity.Campaign;
using CampaignTaboola = Poller.Taboola.Model.Campaign;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Our converter for campaigns.
    /// </summary>
    class MapperCampaign : IMapper<CampaignTaboola, CampaignCore>
    {

        private const string DefaultString = "default";
        private const int DefaultNumber = -1;
        private const string DefaultJson = "{}";
        private const string DefaultCampaignIdNumber = "xxxxxxxx";
        private const string DefaultCampaignIdName = "invalid-campaign-id";
        private const string DefaultCurrency = "XXX";
        private const Delivery DefaultDelivery = Delivery.Unknown;
        private const string DefaultLanguage = "NL";
        private readonly int[] DefaultLocations = new int[0];

        /// <summary>
        /// Converts our core model to taboola campaign.
        /// </summary>
        /// <param name="core">The object to convert</param>
        /// <returns>The converted object</returns>
        public CampaignTaboola Convert(CampaignCore core)
        {
            if (core == null) throw new ArgumentNullException(nameof(core));

            var result = new CampaignTaboola
            {
                Id = core.SecondaryId,
                Name = core.Name,
                BrandingText = core.BrandingText,
                Cpc = core.InitialCpc,
                SpendingLimit = core.Budget,
                DailyCap = core.DailyBudget,
                Spent = core.Spent,
                StartDate = core.StartDate,
                EndDate = core.EndDate,
                Utm = core.Utm,
                Note = core.Note,
            };

            CampaignDetails details = Json.Deserialize
               <CampaignDetails>(core.Details);
            PushDetails(result, details);

            return result;
        }

        /// <summary>
        /// Pushes our details to a given taboola campaign object. This will 
        /// overwrite in all cases.
        /// </summary>
        /// <param name="to">The object to push to</param>
        /// <param name="details">The extracted details</param>
        /// <return>The pushed object with extracted details</return>
        private CampaignTaboola PushDetails(CampaignTaboola to,
            CampaignDetails details)
        {
            if (to == null) throw new ArgumentNullException(nameof(to));
            if (details == null) throw new ArgumentNullException(nameof(details));

            to.Account = details.Account;
            to.DailyAdDeliveryModel = ToUpperString(details.DailyAdDeliveryModel);
            to.PublisherBidModifier = details.PublisherBidModifier;
            to.SpendingLimitModel = ToUpperString(details.SpendingLimitModel);
            to.CountryTargeting = details.CountryTargeting;
            to.SubCountryTargeting = details.SubCountryTargeting;
            to.PostalCodeTargeting = details.PostalCodeTargeting;
            to.ContextualTargeting = details.ContextualTargeting;
            to.PlatformTargeting = details.PlatformTargeting;
            to.OsTargeting = details.OsTargeting;
            to.ConnectionTypeTargeting = details.ConnectionTypeTargeting;
            to.CpaGoal = details.CpaGoal;
            to.BidStrategy = ToUpperString(details.BidStrategy);
            to.TrafficAllocationMode = ToUpperString(details.TrafficAllocationMode);
            to.ApprovalState = ToUpperString(details.ApprovalState); ;
            to.Status = ToUpperString(details.Status);
            to.Active = details.Active;
            to.MarketingObjective = ToUpperString(details.MarketingObjective);

            return to;
        }

        /// <summary>
        /// Converts a taboola campaign to our core model.
        /// </summary>
        /// <param name="external">The object to convert</param>
        /// <returns>The converted object</returns>
        public CampaignCore Convert(CampaignTaboola external)
        {
            if (external == null) throw new ArgumentNullException(nameof(external));

            var result = GetDefault();

            result.SecondaryId = external.Id;
            result.Name = external.Name;
            result.BrandingText = external.BrandingText;
            result.InitialCpc = external.Cpc;
            result.Budget = external.SpendingLimit;
            result.DailyBudget = external.DailyCap;
            result.Spent = external.Spent;
            result.StartDate = external.StartDate;
            result.EndDate = external.EndDate;
            result.Utm = external.Utm;
            result.Note = external.Note;
            result.Details = ExtractDetailsToString(external);

            // Daily cap of 0 means unlimited, which we denote as null
            if (result.DailyBudget == 0) { result.DailyBudget = null; }

            return result;
        }

        /// <summary>
        /// Extracts the details from a Taboola campaign.
        /// This then converts it to a JSON string.
        /// </summary>
        /// <param name="from">The Taboola campaign</param>
        /// <returns>The details object as JSON string</returns>
        private string ExtractDetailsToString(
            CampaignTaboola from)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));

            return Json.Serialize(new CampaignDetails
            {
                Account = from.Account,
                DailyAdDeliveryModel = FromUpperString(from.DailyAdDeliveryModel, DailyAdDeliveryModel.Unknown),
                PublisherBidModifier = from.PublisherBidModifier,
                SpendingLimitModel = FromUpperString(from.SpendingLimitModel, SpendingLimitModel.Entire),
                CountryTargeting = from.CountryTargeting,
                SubCountryTargeting = from.SubCountryTargeting,
                PostalCodeTargeting = from.PostalCodeTargeting,
                ContextualTargeting = from.ContextualTargeting,
                PlatformTargeting = from.PlatformTargeting,
                OsTargeting = from.OsTargeting,
                ConnectionTypeTargeting = from.ConnectionTypeTargeting,
                CpaGoal = from.CpaGoal,
                BidStrategy = FromUpperString(from.BidStrategy, BidType.Fixed),
                TrafficAllocationMode = FromUpperString(from.TrafficAllocationMode, TrafficAllocationMode.Even),
                ApprovalState = FromUpperString(from.ApprovalState, Model.ApprovalState.Pending),
                Status = FromUpperString(from.Status, CampaignStatus.PendingApproval),
                Active = from.Active,
                MarketingObjective = FromUpperString(from.MarketingObjective, MarketingObjective.DriveWebsiteTraffic)
            });
        }

        /// <summary>
        /// Bulk conversion from Taboola to Core.
        /// </summary>
        /// <param name="list">Taboola list</param>
        /// <returns>Core list</returns>
        public IEnumerable<CampaignCore> ConvertAll(
            IEnumerable<CampaignTaboola> list)
        {
            IList<CampaignCore> result = new List<CampaignCore>();
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
        public IEnumerable<CampaignTaboola> ConvertAll(
            IEnumerable<CampaignCore> list)
        {
            IList<CampaignTaboola> result = new List<CampaignTaboola>();
            foreach (var item in list.AsParallel())
            {
                result.Add(Convert(item));
            }
            return result;
        }

        /// <summary>
        /// Creates a default without null values.
        /// TODO Match db schema
        /// TODO Fill up
        /// </summary>
        /// <returns>Default</returns>
        private CampaignCore GetDefault()
        {
            return new CampaignCore
            {
                SecondaryId = DefaultCampaignIdNumber,
                Name = DefaultCampaignIdName,
                LanguageAsText = DefaultLanguage,
                Budget = DefaultNumber,
                DailyBudget = null,
                Delivery = DefaultDelivery,
                LocationInclude = DefaultLocations,
                LocationExclude = DefaultLocations
            };
        }

        /// <summary>
        /// Converts a core campaign to a taboola API campaign. This also sets
        /// all read-only fields to null. This will prevent the API from returning
        /// 400 BAD REQUEST http response codes.
        /// </summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>The converted and nullified converted campaign</returns>
        public CampaignTaboola ConvertAndNullifyReadOnly(CampaignCore campaign)
        {
            var converted = Convert(campaign);
            return NullifyReadOnly(converted);
        }

        /// <summary>
        /// Sets all read-only parameters to null. Use this when you want to
        /// send a campaign to the taboola API for creation or updates. All 
        /// fields which are set to null will be ignored by the Taboola API.
        /// 
        /// The <see cref="CampaignTaboola.ApprovalState"/> is not actually
        /// read only, but requires certain permission to modify. We treat it
        /// as null, so that the API ignores the field.
        /// </summary>
        /// <param name="campaign">Campaign to be sent</param>
        /// <returns>Input campaign with read-only fields set to null</returns>
        private CampaignTaboola NullifyReadOnly(CampaignTaboola campaign)
        {
            campaign.Id = null;
            campaign.Spent = null;
            campaign.Status = null;
            campaign.ApprovalState = null;
            return campaign;
        }

        /// <summary>
        /// Converts an enum to all caps. This is because Taboola uses upper 
        /// case to denote these enums, while we use lower case.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="input">Input enum</param>
        /// <returns>Upper case string</returns>
        private string ToUpperString<T>(T input)
            where T : Enum
        {
            var split = Regex.Split(input.ToString(), @"(?<!^)(?=[A-Z])");

            if (split.Length > 1)
            {
                string result = "";
                for (int i = 0; i < split.Length - 1; i++)
                {
                    result += split[i].ToUpper() + "_";
                }
                result += split[split.Length - 1].ToUpper();
                return result;
            }
            else
            {
                return input.ToString().ToUpper();
            }
        }

        /// <summary>
        /// Attempt to convert an enum from all caps snake case.
        /// </summary>
        /// <typeparam name="TEnum">The enum type</typeparam>
        /// <param name="input">The input string, IN_THIS_FORMAT</param>
        /// <param name="defaultReturn">If we can't parse we return this</param>
        /// <returns>Converted enum</returns>
        public static TEnum FromUpperString<TEnum>(string input, TEnum defaultReturn)
            where TEnum : struct
        {
            if (string.IsNullOrWhiteSpace(input)) { return defaultReturn; }

            var words = input.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            var pascal = "";
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = words[i].Trim('_');
                pascal += words[i].Substring(0, 1).ToUpper() + words[i].Substring(1).ToLower();
            }

            if (Enum.TryParse(pascal, out TEnum result))
            {
                return result;
            }
            else
            {
                return defaultReturn;
            }
        }

    }
}
