
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
    /// </summary>
    class MapperAdItem : IMapperSplit<AdItemTaboola, AdItemCoResult, AdItemCore>
    {

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
        /// </summary>
        /// <param name="from">Core ad item</param>
        /// <returns>Taboola co result ad item</returns>
        public AdItemCoResult Convert(AdItemCore from)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// This converts a core item to a Taboola
        /// ad item. This is the result we get when 
        /// calling the ad item API.
        /// </summary>
        /// <param name="core">Core ad item</param>
        /// <returns>Taboola ad item</returns>
        AdItemTaboola IMapper<AdItemTaboola, AdItemCore>.Convert(AdItemCore core)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// This merges the two types of ad items into
        /// one. TODO Do we need this?
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public AdItemCoResult Merge(AdItemTaboola from, AdItemCoResult to)
        {
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

        /// <summary>
        /// Add Taboola item values to a core ad item.
        /// </summary>
        /// <param name="core">The core ad item</param>
        /// <param name="from">The taboola ad item</param>
        /// <returns>Core ad item with additional values</returns>
        public AdItemCore AddOnto(AdItemCore core, AdItemTaboola from)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Add Taboola co result ad item values to a 
        /// core ad item.
        /// </summary>
        /// <param name="core">The core ad item</param>
        /// <param name="from">The taboola co result ad item</param>
        /// <returns>Core ad item with additional values</returns>
        public AdItemCore AddOnto(AdItemCore core, AdItemCoResult from)
        {
            throw new System.NotImplementedException();
        }


    }
}
