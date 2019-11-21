using Maximiz.ViewModels.AdGroupOverview;
using Maximiz.ViewModels.CampaignOverview;
using System;

namespace Maximiz.Mapper.Enum
{

    /// <summary>
    /// Utility functions for translating enums to their corresponding string values.
    /// </summary>
    internal static class MapperEnum
    {

        /// <summary>
        /// Converts a <see cref="CampaignTableType"/> to its corresponding string value.
        /// </summary>
        /// <param name="table"><see cref="CampaignTableType"/></param>
        /// <returns>String value</returns>
        internal static string TranslateCampaignTableType(CampaignTableType table)
        {
            switch (table)
            {
                case CampaignTableType.All:
                    return "All";
                case CampaignTableType.Active:
                    return "Active";
                case CampaignTableType.Inactive:
                    return "Inactive";
                case CampaignTableType.Pending:
                    return "Pending";
            }

            throw new InvalidOperationException(nameof(table));
        }

        /// <summary>
        /// Converts a <see cref="AdGroupOverviewTableType"/> to its corresponding string value.
        /// </summary>
        /// <param name="table"><see cref="AdGroupOverviewTableType"/></param>
        /// <returns>String value</returns>
        internal static string TranslateCampaignTableType(AdGroupOverviewTableType table)
        {
            switch (table)
            {
                case AdGroupOverviewTableType.All:
                    return "All";
            }

            throw new InvalidOperationException(nameof(table));
        }


    }
}
