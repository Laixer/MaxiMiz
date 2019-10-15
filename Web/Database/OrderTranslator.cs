
namespace Maximiz.Database
{

    /// <summary>
    /// Class to convert an enum to corresponding column name when ordering sql.
    /// </summary>
    internal class OrderTranslator
    {

        /// <summary>
        /// Converts a campaign order to corresponding column name.
        /// </summary>
        /// <remarks>
        /// Default is <see cref="ColumnCampaign.Name"/>.
        /// </remarks>
        /// <param name="input">The column enum</param>
        /// <returns>The column name in the database</returns>
        internal string Convert(ColumnCampaign input)
        {
            switch (input)
            {
                case ColumnCampaign.Name:
                    return "name";
                case ColumnCampaign.Budget:
                    return "budget";
                case ColumnCampaign.StartDate:
                    return "start_date";
                default:
                    return Convert(ColumnCampaign.Name);
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
        internal string Convert( Order order)
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
