using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using System;

namespace Maximiz.Helper
{

    /// <summary>
    /// Used to generate bare minimum entities.
    /// </summary>
    public class BareMinimumModels
    {

        /// <summary>
        /// Creates a default bare minimum campaign.
        /// </summary>
        /// <remarks>
        /// These are all default values:
        /// - name: "default name"
        /// - brandingText: "default branding text"
        /// - language: "XX"
        /// - inititalCpc: 0
        /// - budget: 1
        /// - devices: <see cref="Device.Desktop"/>
        /// - operatingSystems: <see cref="Windows"/>
        /// - connectionTypes: <see cref="ConnectionType.Wifi"/>
        /// - delivery: <see cref="Delivery.Balanced"/>
        /// - bidStrategy: <see cref="BidStrategy.Fixed"/>
        /// - budgetModel: <see cref="BudgetModel.Campaign"/>
        /// </remarks>
        /// <param name="campaignGroup">The campaign group for this campaign</param>
        /// <returns>A new bare minimum campaign</returns>
        public Campaign CreateDefaultBareMinimumCampaign(CampaignGroup campaignGroup)
        {
            return CreateBareMinimumCampaign(
                name: "default name",
                campaignGroup,
                brandingText: "default branding text",
                language: "XX",
                inititalCpc: 0,
                budget: 1,
                devices: new[] { Device.Desktop },
                operatingSystems: new[] { OS.Windows },
                connectionTypes: new[] { ConnectionType.Wifi },
                delivery: Delivery.Balanced,
                bidStrategy: BidStrategy.Fixed,
                budgetModel: BudgetModel.Campaign);
        }

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
        public Campaign CreateBareMinimumCampaign(string name, CampaignGroup campaignGroup,
            string brandingText, string language, decimal inititalCpc, decimal budget,
            Device[] devices, OS[] operatingSystems, ConnectionType[] connectionTypes,
            Delivery delivery, BidStrategy bidStrategy, BudgetModel budgetModel)
        {
            if (inititalCpc < 0) { throw new ArgumentException("Cpc must be > 0"); }
            if (budget < inititalCpc) { throw new ArgumentException("Budget must be > cpc"); }

            return new Campaign
            {
                Name = name,
                CampaignGroupGuid = campaignGroup.Id,
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
                Status = Status.PendingApproval,
                ApprovalState = ApprovalState.Submitted
            };
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
            if (adGroupImageIndex >= adGroup.ImageLinks.Length
                || adGroupImageIndex < 0) { throw new Exception("Image index invalid"); }
            if (adGroupTitleIndex >= adGroup.Titles.Length
                || adGroupTitleIndex < 0) { throw new Exception("Title index invalid"); }

            return new AdItem
            {
                CampaignGuid = campaign.Id,
                AdGroupGuid = adGroup.Id,
                AdGroupImageIndex = adGroupImageIndex,
                AdGroupTitleIndex = adGroupTitleIndex,
                ImageUrl = adGroup.ImageLinks[adGroupImageIndex],
                Title = adGroup.Titles[adGroupTitleIndex],
                Status = Status.PendingApproval,
                ApprovalState = ApprovalState.Submitted
            };
        }

    }
}
