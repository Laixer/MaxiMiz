using Maximiz.Database.Querying;

namespace Maximiz.ViewComponents.AdGroup
{

    /// <summary>
    /// Contains settings for our <see cref="AdGroupTableViewComponent"/>.
    /// </summary>
    public sealed class AdGroupTableSettings
    {

        /// <summary>
        /// Query for our data store.
        /// </summary>
        public QueryAdGroupWithStats Query { get; set; }

        /// <summary>
        /// Determines the kind of clickable functionality our items will get.
        /// </summary>
        public AdGroupTableActionType ActionType { get; set; }

        /// <summary>
        /// If true the name of the items in the table can be clicked on, 
        /// taking us to the campaign group editor.
        /// </summary>
        public bool IsNameClickable { get; set; }

    }
}
