using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using System;

namespace Maximiz.Helper
{

    /// <summary>
    /// Used to generate bare minimum entities.
    /// TODO Sometimes your group or campaign is null. Handle this.
    /// </summary>
    public class BareMinimumModels
    {

        /// <summary>
        /// Creates a bare minimum campaign. All these fields are required in
        /// our own database. If a simple dummy has to be created, call the 
        /// function <see cref="CreateDefaultBareMinimumCampaign(CampaignGroup)"/>
        /// to assign a default value to each parameter.
        /// </summary>
        /// <param name="name">The campaign name</param>
        /// <param name="campaignGroup">The campaign group for this campaign</param>
        /// <param name="brandingText">The brandinig text</param>
        /// <param name="language">The language</param>
        /// <param name="inititalCpc">The initial cost per click</param>
        /// <param name="budget">The total budget</param>
        /// <param name="devices">All selected devices</param>
        /// <param name="operatingSystems">All selected operating systems</param>
        /// <param name="connectionTypes">All selected connection types</param>
        /// <param name="delivery">The delivery model</param>
        /// <param name="bidStrategy">The bid strategy</param>
        /// <param name="budgetModel">The budget model</param>
        /// <returns>A new bare minimum campaign</returns>
        public Campaign CreateBareMinimumCampaign(
            string name,
            CampaignGroup campaignGroup,
            string brandingText = "default branding text",
            string language = "XX",
            decimal inititalCpc = 0,
            decimal budget = 1,
            Device[] devices = null,
            OS[] operatingSystems = null,
            ConnectionType[] connectionTypes = null,
            Delivery delivery = Delivery.Strict,
            BidStrategy bidStrategy = BidStrategy.Fixed,
            BudgetModel budgetModel = BudgetModel.Campaign)
        {
            // Edge cases
            if (inititalCpc < 0) { throw new ArgumentException("Cpc must be > 0"); }
            if (budget < inititalCpc) { throw new ArgumentException("Budget must be > cpc"); }

            // Assign arrays explicitly
            devices = devices ?? new[] { Device.Desktop };
            operatingSystems = operatingSystems ?? new[] { OS.Windows };

            // Create new object
            var result = new Campaign
            {
                Name = name,
                BrandingText = brandingText,
                LanguageAsText = language,
                InitialCpc = inititalCpc,
                Budget = budget,
                Devices = devices,
                OperatingSystems = operatingSystems,
                ConnectionTypes = connectionTypes,
                Delivery = delivery,
                BidStrategy = bidStrategy,
                BudgetModel = budgetModel,
                Status = CampaignStatus.Unknown,
                ApprovalState = ApprovalState.Submitted
            };

            // Only assign if not null
            if (campaignGroup != null) { result.CampaignGroupGuid = campaignGroup.Id; }

            return result;
        }

        /// <summary>
        /// Creates a bare minimum ad item for a given campaign, based on a
        /// provided ad group.
        /// </summary>
        /// <param name="campaign">The corresponding campaign</param>
        /// <param name="adGroup">The corresponding ad group</param>
        /// <param name="adGroupImageIndex">The image index</param>
        /// <param name="adGroupTitleIndex">The title index</param>
        /// <returns>A new bare minimum ad item</returns>
        public AdItem CreateBareMinimumAdItem(Campaign campaign, AdGroup adGroup,
            int adGroupImageIndex, int adGroupTitleIndex)
        {
            if (adGroup != null)
            {
                if (adGroupImageIndex >= adGroup.ImageLinks.Length
                    || adGroupImageIndex < 0) { throw new Exception("Image index invalid"); }
                if (adGroupTitleIndex >= adGroup.Titles.Length
                    || adGroupTitleIndex < 0) { throw new Exception("Title index invalid"); }

            }

            var result = new AdItem
            {
                AdGroupImageIndex = adGroupImageIndex,
                AdGroupTitleIndex = adGroupTitleIndex,
                Status = AdItemStatus.Unknown,
                ApprovalState = ApprovalState.Submitted
            };

            if (campaign != null) { result.CampaignGuid = campaign.Id; }
            if (adGroup != null)
            {
                result.AdGroupGuid = adGroup.Id;
                result.ImageUrl = adGroup.ImageLinks[adGroupImageIndex];
                result.Title = adGroup.Titles[adGroupTitleIndex];
            }

            return result;
        }
    }
}
