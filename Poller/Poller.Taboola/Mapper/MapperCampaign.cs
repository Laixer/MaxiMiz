using System;
using System.Collections.Generic;
using System.Text;
using Maximiz.Model.Entity;
using Poller.Helper;
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

        /// <summary>
        /// Converts our core model to taboola campaign.
        /// </summary>
        /// <param name="from">The object to convert</param>
        /// <returns>The converted object</returns>
        public CampaignTaboola Convert(CampaignCore from)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));

            var result = new CampaignTaboola
            {
                Id = from.SecondaryId,
                Name = from.Name,
                Branding = from.BrandingText,
                Cpc = from.InitialCpc,
                SpendingLimit = from.Budget,
                DailyCap = from.DailyBudget,
                Spent = from.Spent,
                StartDate = from.StartDate,
                EndDate = from.EndDate,
                Utm = from.Utm,
                Note = from.Note,
            };

            CampaignDetails details = Json.Deserialize
               <CampaignDetails>(from.Details);
            PushDetails(result, details);

            return result;
        }

        /// <summary>
        /// Pushes our details to a given taboola
        /// campaign object. This will overwrite in 
        /// all cases.
        /// </summary>
        /// <param name="to">The object to push to</param>
        /// <param name="details">The extracted details</param>
        private void PushDetails(CampaignTaboola to,
            CampaignDetails details)
        {
            if (to == null) throw new ArgumentNullException(nameof(to));
            if (details == null) throw new ArgumentNullException(nameof(details));

            to.Account = details.Account;
            to.DailyAdDeliveryModel = details.DailyAdDeliveryModel;
            to.PublisherBidModifier = details.PublisherBidModifier;
            to.SpendingLimitModel = details.SpendingLimitModel;
            to.CountryTargeting = details.CountryTargeting;
            to.SubCountryTargeting = details.SubCountryTargeting;
            to.PostalCodeTargeting = details.PostalCodeTargeting;
            to.ContextualTargeting = details.ContextualTargeting;
            to.PlatformTargeting = details.PlatformTargeting;
            to.OsTargeting = details.OsTargeting;
            to.ConnectionTypeTargeting = details.ConnectionTypeTargeting;
            to.CpaGoal = details.CpaGoal;
            to.BidStrategy = details.BidStrategy;
            to.TrafficAllocationMode = details.TrafficAllocationMode;
            to.ApprovalState = details.ApprovalState;
            to.Status = details.Status;
            to.Active = details.Active;
        }

        /// <summary>
        /// Converts a taboola campaign to our core model.
        /// </summary>
        /// <param name="from">The object to convert</param>
        /// <returns>The converted object</returns>
        public CampaignCore Convert(CampaignTaboola from)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));

            return new CampaignCore
            {
                SecondaryId = from.Id,
                Name = from.Name,
                BrandingText = from.Branding,
                InitialCpc = from.Cpc,
                Budget = from.SpendingLimit,
                DailyBudget = from.DailyCap,
                Spent = from.Spent,
                StartDate = from.StartDate,
                EndDate = from.EndDate,
                Utm = from.Utm,
                Note = from.Note
            };
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
                DailyAdDeliveryModel = from.DailyAdDeliveryModel,
                PublisherBidModifier = from.PublisherBidModifier,
                SpendingLimitModel = from.SpendingLimitModel,
                CountryTargeting = from.CountryTargeting,
                SubCountryTargeting = from.SubCountryTargeting,
                PostalCodeTargeting = from.PostalCodeTargeting,
                ContextualTargeting = from.ContextualTargeting,
                PlatformTargeting = from.PlatformTargeting,
                OsTargeting = from.OsTargeting,
                ConnectionTypeTargeting = from.ConnectionTypeTargeting,
                CpaGoal = from.CpaGoal,
                BidStrategy = from.BidStrategy,
                TrafficAllocationMode = from.TrafficAllocationMode,
                ApprovalState = from.ApprovalState,
                Status = from.Status,
                Active = from.Active
            });
        }

    }
}
