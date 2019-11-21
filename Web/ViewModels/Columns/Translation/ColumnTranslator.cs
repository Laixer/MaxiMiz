using Maximiz.Database.Columns;
using System;

namespace Maximiz.ViewModels.Columns.Translation
{

    /// <summary>
    /// Translates an internal column to an external column.
    /// </summary>
    internal static class ColumnTranslator
    {

        /// <summary>
        /// Translates a<see cref="ColumnCampaignOverview"/> to the corresponding
        /// <see cref="ColumnCampaignWithStats"/>.
        /// </summary>
        /// <param name="column"><see cref="ColumnCampaignOverview"/></param>
        /// <returns><see cref="ColumnCampaignWithStats"/></returns>
        public static ColumnCampaignWithStats Translate(ColumnCampaignOverview column)
        {
            switch (column)
            {
                case ColumnCampaignOverview.Cpc:
                    return ColumnCampaignWithStats.Cpc;
                case ColumnCampaignOverview.Ctr:
                    return ColumnCampaignWithStats.Ctr;
                case ColumnCampaignOverview.Clicks:
                    return ColumnCampaignWithStats.Clicks;
                case ColumnCampaignOverview.Budget:
                    return ColumnCampaignWithStats.Budget;
                case ColumnCampaignOverview.Spend:
                    return ColumnCampaignWithStats.Spend;
                case ColumnCampaignOverview.Revenue:
                    return ColumnCampaignWithStats.Revenue;
                case ColumnCampaignOverview.Roi:
                    return ColumnCampaignWithStats.Roi;
                case ColumnCampaignOverview.RevenueTaboola:
                    return ColumnCampaignWithStats.RevenueTaboola;
                case ColumnCampaignOverview.RevenueAdsense:
                    return ColumnCampaignWithStats.RevenueAdsense;
                case ColumnCampaignOverview.Profit:
                    return ColumnCampaignWithStats.Profit;
                case ColumnCampaignOverview.Actions:
                    return ColumnCampaignWithStats.Actions;
                case ColumnCampaignOverview.Name:
                    return ColumnCampaignWithStats.Name;
            }

            throw new InvalidOperationException(nameof(column));
        }

        /// <summary>
        /// Translates a <see cref="ColumnAdGroupLinking"/> to the corresponding
        /// <see cref="ColumnAdGroupWithStats"/>.
        /// </summary>
        /// <param name="column"><see cref="ColumnAdGroupLinking"/></param>
        /// <returns><see cref="ColumnAdGroupWithStats"/></returns>
        public static ColumnAdGroupWithStats Translate(ColumnAdGroupLinking column)
        {
            switch (column)
            {
                case ColumnAdGroupLinking.Name:
                    return ColumnAdGroupWithStats.Name;
                case ColumnAdGroupLinking.CreateDate:
                    return ColumnAdGroupWithStats.CreateDate;
                case ColumnAdGroupLinking.UpdateDate:
                    return ColumnAdGroupWithStats.UpdateDate;
                case ColumnAdGroupLinking.AdCount:
                    return ColumnAdGroupWithStats.AdCount;
            }

            throw new InvalidOperationException(nameof(column));
        }

        /// <summary>
        /// Translates a <see cref="ColumnAdGroupOverview"/> to the corresponding
        /// <see cref="ColumnAdGroupWithStats"/>.
        /// </summary>
        /// <param name="column"><see cref="ColumnAdGroupOverview"/></param>
        /// <returns><see cref="ColumnAdGroupWithStats"/></returns>
        public static ColumnAdGroupWithStats Translate(ColumnAdGroupOverview column)
        {
            switch (column)
            {
                case ColumnAdGroupOverview.Name:
                    return ColumnAdGroupWithStats.Name;
                case ColumnAdGroupOverview.CreateDate:
                    return ColumnAdGroupWithStats.CreateDate;
                case ColumnAdGroupOverview.UpdateDate:
                    return ColumnAdGroupWithStats.UpdateDate;
                case ColumnAdGroupOverview.AdCount:
                    return ColumnAdGroupWithStats.AdCount;
            }

            throw new InvalidOperationException(nameof(column));
        }

        /// <summary>
        /// Translates a <see cref="ColumnCampaignGroupWizardAdGroup"/> to the corresponding
        /// <see cref="ColumnAdGroupWithStats"/>.
        /// </summary>
        /// <param name="column"><see cref="ColumnCampaignGroupWizardAdGroup"/></param>
        /// <returns><see cref="ColumnAdGroupWithStats"/></returns>
        public static ColumnAdGroupWithStats Translate(ColumnCampaignGroupWizardAdGroup column)
        {
            switch (column)
            {
                case ColumnCampaignGroupWizardAdGroup.Name:
                    return ColumnAdGroupWithStats.Name;
                case ColumnCampaignGroupWizardAdGroup.CreateDate:
                    return ColumnAdGroupWithStats.CreateDate;
                case ColumnCampaignGroupWizardAdGroup.UpdateDate:
                    return ColumnAdGroupWithStats.UpdateDate;
                case ColumnCampaignGroupWizardAdGroup.AdCount:
                    return ColumnAdGroupWithStats.AdCount;
                case ColumnCampaignGroupWizardAdGroup.Selected:
                    // TODO This must NEVER leave the testing environment
                    throw new NotImplementedException("Can't sort by selected ad group yet");
            }

            throw new InvalidOperationException(nameof(column));
        }

    }
}
