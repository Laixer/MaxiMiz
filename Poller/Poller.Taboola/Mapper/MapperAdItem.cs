
using Maximiz.Model.Entity;
using Poller.Taboola.Model;
using AdItemCore = Maximiz.Model.Entity.AdItem;
using AdItemTaboola = Poller.Taboola.Model.AdItem;
using AdItemCoResult = Poller.Taboola.Model.AdItemCoResult;
using System;
using Poller.Helper;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Our Ad Item mapper.
    /// TODO Details handling might be possible
    /// in a more elegant manner.
    /// </summary>
    class MapperAdItem : IMapperSplit<AdItemTaboola, AdItemCoResult, AdItemCore>
    {

        private const string DefaultString = "INVALID";

        /// <summary>
        /// This converts a Taboola co result ad item 
        /// to our core ad item. This is the result we
        /// get when calling the ad items API. This only
        /// maps the available parameters.
        /// 
        /// TODO Long to int conversion
        /// </summary>
        /// <param name="from">Taboola co result</param>
        /// <returns>Core ad item</returns>
        public AdItemCore Convert(AdItemCoResult from)
        {
            if (from == null) throw new
                    ArgumentNullException(nameof(from));

            return new AdItemCore
            {
                SecondaryId = from.Id,
                Title = from.Title,
                Url = from.Url,
                Cpc = from.Cpc,
                Spent = from.Spent,
                Impressions = (int)from.Impressions,
                Actions = (int)from.Actions,
                Details = ExtractDetailsToString(from)
            };
        }

        /// <summary>
        /// Extracts the details from a Taboola co result.
        /// This then converts it to a JSON string.
        /// </summary>
        /// <param name="from">The Taboola co result</param>
        /// <returns>The details object as JSON string</returns>
        private string ExtractDetailsToString(
            AdItemCoResult from)
        {
            if (from == null) throw new
                    ArgumentNullException(nameof(from));

            return Json.Serialize(new AdItemDetails
            {
                ThumbnailUrl = from.ThumbnailUrl,
                CampaignName = from.CampaignName,
                ContentProvider = from.ContentProvider,
                ContentProviderName = from.ContentProviderName,
                Clicks = from.Clicks,
                Cpm = from.Cpm,
                Currency = from.Currency,
                Cpa = from.Cpa,
                Cvr = from.Cvr
            });
        }

        /// <summary>
        /// This converts a Taboola ad item to our
        /// core ad item. This is the result we get 
        /// when we call the report API. This only
        /// maps the available parameters.
        /// </summary>
        /// <param name="external">The Taboola ad item</param>
        /// <returns>The core ad item</returns>
        public AdItemCore Convert(AdItemTaboola external)
        {
            if (external == null) throw new
                    ArgumentNullException(nameof(external));

            return new AdItemCore
            {
                SecondaryId = external.Id,
                Title = external.Title,
                Url = external.Url,
                Details = ExtractDetailsToString(external)
            };
        }

        /// <summary>
        /// Extracts the details from a Taboola ad item.
        /// This then converts it to a JSON string.
        /// </summary>
        /// <param name="from">The Taboola ad item</param>
        /// <returns>The details object as JSON string</returns>
        private string ExtractDetailsToString(
            AdItemTaboola from)
        {
            if (from == null) throw new
                    ArgumentNullException(nameof(from));

            return Json.Serialize(new AdItemDetails
            {
                CampaignId = from.CampaignId,
                Active = from.Active,
                ApprovalState = from.ApprovalState,
                CampaignItemStatus = from.CampaignItemStatus
            });
        }

        /// <summary>
        /// This converts a core item to a Taboola
        /// co result. This is the result we get 
        /// when calling the report API.
        /// 
        /// TODO Null handling because of split instance.
        /// </summary>
        /// <param name="from">Core ad item</param>
        /// <returns>Taboola co result ad item</returns>
        public AdItemCoResult Convert(AdItemCore from)
        {
            if (from == null) throw new
                    ArgumentNullException(nameof(from));

            AdItemDetails details = Json.Deserialize
                <AdItemDetails>(from.Details);

            return new AdItemCoResult
            {
                // Properties
                Id = from.SecondaryId,
                Title = from.Title,
                Url = from.Url,
                Cpc = from.Cpc,
                Spent = from.Spent,
                Impressions = from.Impressions,
                Actions = from.Actions,

                // Details
                Campaign = details.CampaignId,
                ThumbnailUrl = details.ThumbnailUrl,
                CampaignName = details.CampaignName,
                ContentProvider = details.ContentProvider,
                ContentProviderName = details.ContentProviderName,
                Clicks = details.Clicks,
                Cpm = details.Cpm,
                Currency = details.Currency,
                Cpa = details.Cpa,
                Cvr = details.Cvr

            };
        }

        /// <summary>
        /// This converts a core item to a Taboola
        /// ad item. This is the result we get when 
        /// calling the ad item API.
        /// 
        /// TODO Null handling
        /// </summary>
        /// <param name="core">Core ad item</param>
        /// <returns>Taboola ad item</returns>
        AdItemTaboola IMapper<AdItemTaboola, AdItemCore>
            .Convert(AdItemCore core)
        {
            if (core == null) throw new
                    ArgumentNullException(nameof(core));

            AdItemDetails details = Json.Deserialize
                <AdItemDetails>(core.Details);

            return new AdItemTaboola
            {
                // Properties
                Id = core.SecondaryId,
                Title = core.Title,
                Url = core.Url,

                // Details
                CampaignId = details.CampaignId,
                Active = details.Active,
                ApprovalState = details.ApprovalState,
                CampaignItemStatus = details.CampaignItemStatus
            };
        }

        /// <summary>
        /// Add Taboola item values to a core ad item.
        /// 
        /// TODO Null handling & overwrite handling
        /// </summary>
        /// <param name="core">The core ad item</param>
        /// <param name="from">The taboola ad item</param>
        /// <returns>Core ad item with additional values</returns>
        public AdItemCore AddOnto(AdItemCore core, AdItemTaboola from)
        {
            if (core == null) throw new
                    ArgumentNullException(nameof(core));
            if (from == null) throw new
                    ArgumentNullException(nameof(from));

            // Check if both items have the same id
            if (core.SecondaryId != null &&
                from.Id != null &&
                core.SecondaryId != from.Id)
                throw new InvalidOperationException(
                    "Can't add ad items with different id's");

            // Add
            // TODO Should we overwrite the secondary id?
            core.SecondaryId = from.Id;
            core.Title = from.Title;
            core.Url = from.Url;

            // Details
            AdItemDetails details = Json.Deserialize
                <AdItemDetails>(core.Details);
            details.CampaignId = from.CampaignId;
            details.Active = from.Active;
            details.ApprovalState = from.ApprovalState;
            details.CampaignItemStatus = from.CampaignItemStatus;
            core.Details = Json.Serialize(details);

            return core;
        }

        /// <summary>
        /// Add Taboola co result ad item values to a 
        /// core ad item.
        /// 
        /// TODO Null handling & overwrite handling
        /// </summary>
        /// <param name="core">The core ad item</param>
        /// <param name="from">The taboola co result ad item</param>
        /// <returns>Core ad item with additional values</returns>
        public AdItemCore AddOnto(AdItemCore core, AdItemCoResult from)
        {
            if (core == null) throw new
                    ArgumentNullException(nameof(core));
            if (from == null) throw new
                    ArgumentNullException(nameof(from));

            // Check if both items have the same id
            if (core.SecondaryId != null &&
                from.Id != null &&
                core.SecondaryId != from.Id)
                throw new InvalidOperationException(
                    "Can't add ad items with different id's");

            // Add
            // TODO Should we overwrite the secondary id?
            core.SecondaryId = from.Id;
            core.Title = from.Title;
            core.Url = from.Url;
            core.Cpc = from.Cpc;
            core.Spent = from.Spent;
            core.Impressions = (int)from.Impressions;
            core.Actions = (int)from.Actions;

            // Details
            AdItemDetails details = Json.Deserialize
                <AdItemDetails>(core.Details);
            details.CampaignId = from.Campaign;
            details.ThumbnailUrl = from.ThumbnailUrl;
            details.CampaignName = from.CampaignName;
            details.ContentProvider = from.ContentProvider;
            details.ContentProviderName = from.ContentProviderName;
            details.Clicks = from.Clicks;
            details.Cpm = from.Cpm;
            details.Currency = from.Currency;
            details.Cpa = from.Cpa;
            details.Cvr = from.Cvr;
            core.Details = Json.Serialize(details);

            return core;
        }

        // TODO This is just re-using our current functions.
        /// <summary>
        /// This merges the two types of ad items into
        /// one. TODO Do we need this?
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public AdItemCoResult Merge(
            AdItemTaboola from, AdItemCoResult to)
        {
            if (from == null) throw new
                    ArgumentNullException(nameof(from));
            if (to == null) throw new
                    ArgumentNullException(nameof(to));

            throw new System.NotImplementedException();
        }

        /// <summary>
        /// TODO Do we need this?
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public AdItemTaboola Merge(AdItemCoResult from, AdItemTaboola to)
        {
            throw new System.NotImplementedException();
        }

    }
}
