using Poller.Taboola.Model;
using System;
using Poller.Helper;
using System.Collections.Generic;
using System.Linq;
using Maximiz.Helper;

using AdItemInternal = Maximiz.Model.Entity.AdItem;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Our Ad Item mapper. This section of the partial class contains the main
    /// conversion functions.
    /// </summary>
    internal partial class MapperAdItem : IMapperAdditional<AdItemExternal, AdItemInternal, AdItemReports>
    {

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
        public MapperAdItem()
        {
            _utility = new MapperUtility();
            _bareMinimumInternal = new BareMinimumModels();
            _bareMinimumExternal = new BareMinimumTaboola();
        }

        /// <summary>
        /// Convert a single ad item while conserving the guid.
        /// </summary>
        /// <param name="external">The external ad item</param>
        /// <param name="guid">The internal ad item GUID</param>
        /// <param name="campaignGuid">The internal campaign GUID</param>
        /// <returns>Internal ad item with all properties and guid</returns>
        public AdItemInternal Convert(AdItemExternal external, Guid guid, Guid campaignGuid)
        {
            var converted = Convert(external);
            converted.Id = guid;
            converted.CampaignGuid = campaignGuid;
            return converted;
        }

        /// <summary>
        /// This converts a Taboola ad item to our core ad item. This is the 
        /// result we get  when we call the report API. This only maps the 
        /// available parameters.
        /// </summary>
        /// <param name="external">The Taboola ad item</param>
        /// <returns>The core ad item</returns>
        public AdItemInternal Convert(AdItemExternal external)
        {
            if (external == null) throw new
                    ArgumentNullException(nameof(external));

            // Create base
            var result = _bareMinimumInternal.CreateBareMinimumAdItem(
                campaign: null,
                adGroup: null,
                adGroupImageIndex: -1,
                adGroupTitleIndex: -1,
                title: external.Title,
                url: external.Url);

            // Append values
            result.SecondaryId = external.Id;
            result.Url = external.Url;
            result.ImageUrl = external.ThumbnailUrl;
            result.ApprovalState = _utility.ApprovalStateToInternal(external.ApprovalState);
            result.Status = AdItemStatusToInternal(external.AdItemStatus);

            // Append details
            result.Details = ExtractDetailsToString(external);

            // TODO Fic to counter "the title violates null constraint bug"
            if (string.IsNullOrEmpty(result.Title)) { result.Title = ""; }

            return result;
        }

        /// <summary>
        /// This converts a core item to a Taboola ad item. This is the result 
        /// we get when calling the ad item API.
        /// </summary>
        /// <param name="core">Internal ad item</param>
        /// <returns>Taboola ad item</returns>
        public AdItemExternal Convert(AdItemInternal core)
        {
            // Edge cases (TODO)

            // Create bare minimum
            var result = _bareMinimumExternal.CreateBareMinimumAdItem(url: core.Url);

            // Append values
            result.Id = core.SecondaryId;
            result.Url = core.Url;
            result.ThumbnailUrl = core.ImageUrl;
            result.Title = core.Title;
            result.ApprovalState = _utility.ApprovalStateToString(core.ApprovalState);
            result.AdItemStatus = AdItemStatusToString(core.Status);

            // Extract details
            var details = Json.Deserialize<AdItemDetails>(core.Details ?? "{}");
            result.Active = details.Active;
            result.Type = details.Type;
            result.CampaignId = details.CampaignId;

            return result;
        }

        /// <summary>
        /// TODO Doc
        /// </summary>
        /// <param name="external"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public AdItemInternal ConvertAdditional(AdItemReports external, Guid guid)
        {
            var converted = ConvertAdditional(external);
            converted.Id = guid;
            return converted;
        }

        /// <summary>
        /// This converts a Taboola co result ad item 
        /// to our core ad item. This is the result we
        /// get when calling the ad items API. This only
        /// maps the available parameters.
        /// 
        /// TODO Long to int conversion
        /// </summary>
        /// <param name="external">Taboola co result</param>
        /// <returns>Core ad item</returns>
        public AdItemInternal ConvertAdditional(AdItemReports external)
        {
            if (external == null) throw new
                    ArgumentNullException(nameof(external));

            // Create base
            var result = _bareMinimumInternal.CreateBareMinimumAdItem(
                campaign: null,
                adGroup: null,
                adGroupImageIndex: -1,
                adGroupTitleIndex: -1,
                title: external.Title,
                url: external.Url);

            // Append values
            result.SecondaryId = external.Id;
            result.Url = external.Url;
            result.ImageUrl = external.ThumbnailUrl;
            result.Spent = external.Spent;
            result.Clicks = (int)external.Clicks;
            result.Impressions = (int)external.Impressions;
            result.Actions = (int)external.Actions;
            result.Cpc = external.Cpc;

            // Append details
            result.Details = ExtractDetailsToString(external);

            return result;
        }

        /// <summary>
        /// Extracts the details from a Taboola reports result. This then converts 
        /// it to a JSON string.
        /// </summary>
        /// <param name="from">The Taboola reports result</param>
        /// <returns>The details object as JSON string</returns>
        private string ExtractDetailsToString(AdItemReports from)
        {
            if (from == null) throw new
                    ArgumentNullException(nameof(from));

            return Json.Serialize(new AdItemDetails
            {
                CampaignId = from.Campaign,
                CampaignName = from.CampaignName,
                ContentProvider = from.ContentProvider,
                ContentProviderName = from.ContentProviderName,
                Currency = from.Currency
            });
        }

        /// <summary>
        /// Extracts the details from a Taboola ad item. This then converts it 
        /// to a JSON string.
        /// </summary>
        /// <param name="from">The Taboola ad item</param>
        /// <returns>The details object as JSON string</returns>
        private string ExtractDetailsToString(AdItemExternal from)
        {
            if (from == null) throw new
                    ArgumentNullException(nameof(from));

            return Json.Serialize(new AdItemDetails
            {
                CampaignId = from.CampaignId,
                Active = from.Active,
                Type = from.Type
            });
        }

        /// <summary>
        /// Convert a list from taboola to core.
        /// </summary>
        /// <param name="list">Taboola items</param>
        /// <returns>Core items</returns>
        public IEnumerable<AdItemInternal> ConvertAll(IEnumerable<AdItemExternal> list)
        {
            List<AdItemInternal> result = new List<AdItemInternal>();
            foreach (var x in list.AsParallel())
            {
                result.Add(Convert(x));
            }
            return result;
        }

        /// <summary>
        /// Convert a list from taboola to core.
        /// </summary>
        /// <param name="list">Taboola items</param>
        /// <returns>Core items</returns>
        public IEnumerable<AdItemInternal> ConvertAllAdditional(IEnumerable<AdItemReports> list)
        {
            List<AdItemInternal> result = new List<AdItemInternal>();
            foreach (var x in list)
            {
                result.Add(ConvertAdditional(x));
            }
            return result;
        }

        /// <summary>
        /// TODO Duplicate function
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public IEnumerable<AdItemExternal> ConvertAll(IEnumerable<AdItemInternal> list)
        {
            List<AdItemExternal> result = new List<AdItemExternal>();
            foreach (var x in list)
            {
                result.Add(Convert(x));
            }
            return result;
        }

    }
}
