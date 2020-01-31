using Maximiz.Core.Querying;
using Maximiz.Model.Entity;
using Maximiz.ViewModels.Columns;
using Order = Maximiz.ViewModels.Columns.Order;

namespace Maximiz.QueryTranslation
{

    /// <summary>
    /// Contract for translating queries.
    /// </summary>
    public interface IQueryTranslator
    {

        QueryBase<AdGroupWithStats> Translate(ColumnAdGroupOverview column, Order order, string searchString = null, int page = 1);

        QueryBase<AdGroupWithStats> Translate(ColumnAdGroupLinking column, Order order, string searchString = null, int page = 1);

        QueryBase<AdGroupWithStats> Translate(ColumnCampaignGroupWizardAdGroup column, Order order, string searchString = null, int page = 1);

        QueryBase<CampaignWithStats> Translate(ColumnCampaignOverview column, Order order, string searchString = null, int page = 1);

    }
}
