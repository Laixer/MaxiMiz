using Maximiz.Database.Columns;

namespace Maximiz.Database.Mapper
{

    /// <summary>
    /// Class to convert an enum to corresponding column name when ordering sql.
    /// </summary>
    public class QueryTranslator
    {

        /// <summary>
        /// Converts a campaign column enum to the corresponding name that 
        /// is used in our database.
        /// </summary>
        /// <remarks>
        /// Default is <see cref="ColumnCampaign.Name"/>.
        /// </remarks>
        /// <param name="column">The column enum</param>
        /// <returns>The column name used in the database</returns>
        public string Convert(ColumnCampaign column)
        {
            switch (column)
            {
                case ColumnCampaign.Name:
                    return "name";
                case ColumnCampaign.Budget:
                    return "budget";
                case ColumnCampaign.StartDate:
                    return "start_date";
                case ColumnCampaign.Status:
                    return "status";
                default:
                    return Convert(ColumnCampaign.Name);
            }
        }

        /// <summary>
        /// Converts a campaign column enum to the corresponding name that 
        /// is used in our database.
        /// </summary>
        /// <remarks>
        /// Default is <see cref="ColumnCampaignWithStats.Name"/>.
        /// </remarks>
        /// <param name="column">The column enum</param>
        /// <returns>The column name used in the database</returns>
        public string Convert(ColumnCampaignWithStats column)
        {
            switch (column)
            {
                case ColumnCampaignWithStats.Name:
                    return "name";
                case ColumnCampaignWithStats.Budget:
                    return "budget";
                case ColumnCampaignWithStats.StartDate:
                    return "start_date";
                case ColumnCampaignWithStats.Status:
                    return "status";
                default:
                    return Convert(ColumnCampaignWithStats.Name);
            }
        }

        /// <summary>
        /// Converts a campaign group column enum to the corresponding name that 
        /// is used in our database.
        /// </summary>
        /// <remarks>
        /// Default is <see cref="ColumnCampaignGroup.Name"/>.
        /// </remarks>
        /// <param name="column">The column enum</param>
        /// <returns>The column name used in the database</returns>
        public string Convert (ColumnCampaignGroup column)
        {
            switch (column)
            {
                case ColumnCampaignGroup.Name:
                    return "name";
                default:
                    return Convert(ColumnCampaignGroup.Name);
            }
        }

        /// <summary>
        /// Converts a campaign group column enum to the corresponding name that 
        /// is used in our database.
        /// </summary>
        /// <remarks>
        /// Default is <see cref="ColumnCampaignGroupWithStats.Name"/>.
        /// </remarks>
        /// <param name="column">The column enum</param>
        /// <returns>The column name used in the database</returns>
        public string Convert(ColumnCampaignGroupWithStats column)
        {
            switch (column)
            {
                case ColumnCampaignGroupWithStats.Name:
                    return "name";
                default:
                    return Convert(ColumnCampaignGroupWithStats.Name);
            }
        }

        /// <summary>
        /// Converts an ad item column enum to the corresponding name that 
        /// is used in our database.
        /// </summary>
        /// <remarks>
        /// Default is <see cref="ColumnAdItem.Name"/>.
        /// </remarks>
        /// <param name="column">The column enum</param>
        /// <returns>The column name used in the database</returns>
        public string Convert(ColumnAdItem column)
        {
            switch (column)
            {
                case ColumnAdItem.Name:
                    return "name";
                default:
                    return Convert(ColumnAdItem.Name);
            }
        }

        /// <summary>
        /// Converts an ad item column enum to the corresponding name that 
        /// is used in our database.
        /// </summary>
        /// <remarks>
        /// Default is <see cref="ColumnAdItemWithStats.Name"/>.
        /// </remarks>
        /// <param name="column">The column enum</param>
        /// <returns>The column name used in the database</returns>
        public string Convert(ColumnAdItemWithStats column)
        {
            switch (column)
            {
                case ColumnAdItemWithStats.Name:
                    return "name";
                default:
                    return Convert(ColumnAdItemWithStats.Name);
            }
        }

        /// <summary>
        /// Converts an ad group column enum to the corresponding name that 
        /// is used in our database.
        /// </summary>
        /// <remarks>
        /// Default is <see cref="ColumnAdGroup.Name"/>.
        /// </remarks>
        /// <param name="column">The column enum</param>
        /// <returns>The column name used in the database</returns>
        public string Convert(ColumnAdGroup column)
        {
            switch (column)
            {
                case ColumnAdGroup.Name:
                    return "name";
                default:
                    return Convert(ColumnAdGroup.Name);
            }
        }

        /// <summary>
        /// Converts an ad group column enum to the corresponding name that 
        /// is used in our database.
        /// </summary>
        /// <remarks>
        /// Default is <see cref="ColumnAdGroupWithStats.Name"/>.
        /// </remarks>
        /// <param name="column">The column enum</param>
        /// <returns>The column name used in the database</returns>
        public string Convert(ColumnAdGroupWithStats column)
        {
            switch (column)
            {
                case ColumnAdGroupWithStats.Name:
                    return "name";
                default:
                    return Convert(ColumnAdGroupWithStats.Name);
            }
        }

        /// <summary>
        /// Converts an order to corresponding sql string.
        /// </summary>
        /// <remarks>
        /// Default is <see cref="Order.Descending"/>.
        /// </remarks>
        /// <param name="order">The order enum</param>
        /// <returns>The order as string</returns>
        public string Convert(Order order)
        {
            switch (order)
            {
                case Order.Ascending:
                    return "ASC";
                case Order.Descending:
                    return "DESC";
                default:
                    return Convert(Order.Descending);
            }
        }

    }
}
