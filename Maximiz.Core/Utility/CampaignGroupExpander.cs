using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using System;
using System.Collections.Generic;

namespace Maximiz.Core.Utility
{

    /// <summary>
    /// Contains functionality to generate <see cref="Campaign"/>s from a 
    /// <see cref="CampaignGroup"/>.
    /// </summary>
    public static class CampaignGroupExpander
    {

        /// <summary>
        /// Generates a <see cref="Campaign"/> based on all the possible
        /// combinations of a <see cref="CampaignGroup"/>.
        /// </summary>
        /// <param name="campaignGroup"><see cref="CampaignGroup"/></param>
        /// <returns></returns>
        public static IEnumerable<Campaign> Expand(CampaignGroup campaignGroup)
        {
            if (campaignGroup == null) { throw new ArgumentNullException(nameof(campaignGroup)); }
            if (campaignGroup.OperationId == null || campaignGroup.OperationId == Guid.Empty) { throw new ArgumentNullException(nameof(campaignGroup.OperationId)); }

            var result = new List<Campaign>();
            foreach (var locationInteger in campaignGroup.LocationInclude)
            {
                foreach (var device in campaignGroup.Devices)
                {
                    foreach (var operatingSystem in campaignGroup.OperatingSystems)
                    {
                        result.Add(new Campaign
                        {
                            AccountGuid = campaignGroup.AccountId,
                            BidStrategy = campaignGroup.BidStrategy,
                            BrandingText = campaignGroup.BrandingText,
                            Budget = campaignGroup.Budget,
                            BudgetDaily = campaignGroup.BudgetDaily,
                            BudgetModel = campaignGroup.BudgetModel,
                            CampaignGroupGuid = campaignGroup.Id,
                            // ConnectionTypes = campaignGroup. TODO Skipped
                            Delivery = campaignGroup.Delivery,
                            Devices = new[] { device },
                            EndDate = campaignGroup.EndDate,
                            InitialCpc = campaignGroup.InitialCpc,
                            Language = campaignGroup.Language,
                            LocationExclude = new int[0], // TODO How to do this
                            LocationInclude = new[] { locationInteger },
                            Name = EntityNameGenerator.ForCampaignFromCampaignGroup(campaignGroup, MapperLocationIntegers.Map(locationInteger), device, operatingSystem),
                            Note = "", // TODO Ignore maybe?
                            OperatingSystems = new[] { operatingSystem },
                            OperationId = campaignGroup.OperationId,
                            OperationItemStatus = OperationItemStatus.PendingCreate,
                            Publisher = campaignGroup.Publisher,
                            StartDate = campaignGroup.StartDate,
                            TargetUrl = campaignGroup.TargetUrl,
                            Utm = UtmGenerator.ForCampaignFromCampaignGroup(campaignGroup)
                        });
                    }
                }
            }

            return result;
        }

    }
}
