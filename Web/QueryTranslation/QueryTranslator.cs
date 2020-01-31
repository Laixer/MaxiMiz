using Maximiz.Core.Querying;
using Maximiz.Model.Entity;
using Maximiz.ViewModels.Columns;
using Order = Maximiz.ViewModels.Columns.Order;

namespace Maximiz.QueryTranslation
{

    /// <summary>
    /// Translates our viewmodel columns and sorting to a proper <see cref="QueryBase{TEntity}"/>.
    /// TODO DRY
    /// </summary>
    public sealed class QueryTranslator : IQueryTranslator
    {

        private readonly ColumnMapper columnMapper;
        private readonly OrderMapper orderMapper;

        /// <summary>
        /// Constructor for dependency injection and creation.
        /// </summary>
        public QueryTranslator()
        {
            columnMapper = new ColumnMapper();
            orderMapper = new OrderMapper();
        }

        public QueryBase<AdGroupWithStats> Translate(ColumnAdGroupOverview column, Order order, string searchString = null, int page = 1)
            => new QueryBase<AdGroupWithStats>
            {
                SortableProperty = columnMapper.Map(column),
                Order = orderMapper.Map(order),
                SearchString = searchString,
                Page = page
            };

        public QueryBase<AdGroupWithStats> Translate(ColumnAdGroupLinking column, Order order, string searchString = null, int page = 1)
            => new QueryBase<AdGroupWithStats>
            {
                SortableProperty = columnMapper.Map(column),
                Order = orderMapper.Map(order),
                SearchString = searchString,
                Page = page
            };

        public QueryBase<AdGroupWithStats> Translate(ColumnCampaignGroupWizardAdGroup column, Order order, string searchString = null, int page = 1)
            => new QueryBase<AdGroupWithStats>
            {
                SortableProperty = columnMapper.Map(column),
                Order = orderMapper.Map(order),
                SearchString = searchString,
                Page = page
            };

        public QueryBase<CampaignWithStats> Translate(ColumnCampaignOverview column, Order order, string searchString = null, int page = 1)
            => new QueryBase<CampaignWithStats>
            {
                SortableProperty = columnMapper.Map(column),
                Order = orderMapper.Map(order),
                SearchString = searchString,
                Page = page
            };

    }
}
