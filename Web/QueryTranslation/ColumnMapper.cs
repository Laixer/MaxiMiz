using Maximiz.Model.Entity;
using Maximiz.ViewModels.Columns;
using System;
using System.Linq.Expressions;

namespace Maximiz.QueryTranslation
{

    /// <summary>
    /// Maps columns to expressions.
    /// </summary>
    internal sealed class ColumnMapper
    {

        /// <summary>
        /// Maps a <see cref="ColumnAdGroupOverview"/> to the respective
        /// <see cref="AdGroupWithStats"/> property expression.
        /// </summary>
        /// <param name="column"><see cref="ColumnAdGroupOverview"/></param>
        /// <returns><see cref="Expression"/></returns>
        internal Expression<Func<AdGroupWithStats, object>> Map(ColumnAdGroupOverview column)
        {
            switch (column)
            {
                case ColumnAdGroupOverview.Name:
                    return x => x.Name;
                case ColumnAdGroupOverview.CreateDate:
                    return x => x.CreateDate;
                case ColumnAdGroupOverview.UpdateDate:
                    return x => x.UpdateDate;
                case ColumnAdGroupOverview.AdCount:
                    return x => x.AdItemCount;
            }

            throw new InvalidOperationException(nameof(column));
        }

        /// <summary>
        /// Maps a <see cref="ColumnAdGroupLinking"/> to the respective
        /// <see cref="AdGroupWithStats"/> property expression.
        /// </summary>
        /// <param name="column"><see cref="ColumnAdGroupLinking"/></param>
        /// <returns><see cref="Expression"/></returns>
        internal Expression<Func<AdGroupWithStats, object>> Map(ColumnAdGroupLinking column)
        {
            switch (column)
            {
                case ColumnAdGroupLinking.Name:
                    return x => x.Name;
                case ColumnAdGroupLinking.CreateDate:
                    return x => x.CreateDate;
                case ColumnAdGroupLinking.UpdateDate:
                    return x => x.UpdateDate;
                case ColumnAdGroupLinking.AdCount:
                    return x => x.AdItemCount;
            }

            throw new InvalidOperationException(nameof(column));
        }

        /// <summary>
        /// Maps a <see cref="ColumnCampaignGroupWizardAdGroup"/> to the respective
        /// <see cref="AdGroupWithStats"/> property expression.
        /// </summary>
        /// <param name="column"><see cref="ColumnCampaignGroupWizardAdGroup"/></param>
        /// <returns><see cref="Expression"/></returns>
        internal Expression<Func<AdGroupWithStats, object>> Map(ColumnCampaignGroupWizardAdGroup column)
        {
            switch (column)
            {
                case ColumnCampaignGroupWizardAdGroup.Name:
                    return x => x.Name;
                case ColumnCampaignGroupWizardAdGroup.CreateDate:
                    return x => x.CreateDate;
                case ColumnCampaignGroupWizardAdGroup.UpdateDate:
                    return x => x.UpdateDate;
                case ColumnCampaignGroupWizardAdGroup.AdCount:
                    return x => x.AdItemCount;
                case ColumnCampaignGroupWizardAdGroup.Selected:
                    throw new NotImplementedException($"We currently can't sort by the {nameof(ColumnCampaignGroupWizardAdGroup.Selected)} column");
            }

            throw new InvalidOperationException(nameof(column));
        }

        /// <summary>
        /// Maps a <see cref="ColumnCampaignOverview"/> to the respective
        /// <see cref="CampaignWithStats"/> property expression.
        /// </summary>
        /// <param name="column"><see cref="ColumnAdGroupOverview"/></param>
        /// <returns><see cref="Expression"/></returns>
        internal Expression<Func<CampaignWithStats, object>> Map(ColumnCampaignOverview column)
        {
            switch (column)
            {
                case ColumnCampaignOverview.Name:
                    return x => x.Name;
                case ColumnCampaignOverview.Cpc:
                    return x => x.InitialCpc;
                case ColumnCampaignOverview.Ctr:
                    return x => x.Ctr;
                case ColumnCampaignOverview.Clicks:
                    return x => x.Clicks; 
                case ColumnCampaignOverview.Budget:
                    return x => x.Budget; 
                case ColumnCampaignOverview.Spend:
                    return x => x.Spent;
                case ColumnCampaignOverview.Revenue:
                    return x => x.Revenue;
                case ColumnCampaignOverview.Roi:
                    return x => x.Roi;
                case ColumnCampaignOverview.RevenueTaboola:
                    return x => x.RevenueTaboola;
                case ColumnCampaignOverview.RevenueAdsense:
                    return x => x.RevenueAdsense;
                case ColumnCampaignOverview.Profit:
                    return x => x.Profit;
                case ColumnCampaignOverview.Actions:
                    return x => x.Actions;
            }

            throw new InvalidOperationException(nameof(column));
        }

    }
}
