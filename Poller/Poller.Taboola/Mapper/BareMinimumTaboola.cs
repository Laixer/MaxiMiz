using Poller.Taboola.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Generates bare minimum entities that we are allowed to send to tthe 
    /// Taboola API.
    /// </summary>
    internal class BareMinimumTaboola
    {

        /// <summary>
        /// Used for utility functions.
        /// </summary>
        private MapperUtility _utility;

        /// <summary>
        /// Constructor to setup object creation.
        /// </summary>
        public BareMinimumTaboola()
        {
            _utility = new MapperUtility();
        }

        /// <summary>
        /// Creates a new bare minimal campaign.
        /// </summary>
        /// <remarks>
        /// Default parameters:
        /// - name: "default name"
        /// - brandingText: "default branding text"
        /// - cpc: 0
        /// - spendingLimit: 1
        /// - spendingLimitModel: <see cref="SpendingLimitModel.Entire"/>
        /// - marketingObjective: <see cref="MarketingObjective.DriveWebsiteTraffic"/>
        /// </remarks>
        /// <param name="name">The campaign name</param>
        /// <param name="brandingText">The branding text</param>
        /// <param name="cpc">The cpc</param>
        /// <param name="spendingLimit">The spending limit (max budget)</param>
        /// <param name="spendingLimitModel">The spending limit model</param>
        /// <param name="marketingObjective">The marketing objective</param>
        /// <returns>A new bare minimal campaign</returns>
        public Campaign CreateBareMinimumCampaign(
            string name = "default name",
            string brandingText = "default branding text",
            decimal cpc = 0,
            decimal spendingLimit = 1,
            SpendingLimitModel spendingLimitModel = SpendingLimitModel.Entire,
            MarketingObjective marketingObjective = MarketingObjective.DriveWebsiteTraffic)
        {
            return new Campaign
            {
                Name = name,
                BrandingText = brandingText,
                Cpc = cpc,
                SpendingLimit = spendingLimit,
                SpendingLimitModel = _utility.ToUpperString(spendingLimitModel),
                MarketingObjective = _utility.ToUpperString(marketingObjective)
            };
        }

        /// <summary>
        /// Only the url is required to create a new ad item. This is only to
        /// be used within the creation API, not in the reports API (we can't).
        /// </summary>
        /// <param name="url">The target url</param>
        /// <returns>A new bare minimum ad item</returns>
        public AdItemMain CreateBareMinimumAdItem(string url)
        {
            return new AdItemMain
            {
                Url = url
            };
        }

    }
}
