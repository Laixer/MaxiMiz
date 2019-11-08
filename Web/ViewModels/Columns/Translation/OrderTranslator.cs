using OrderViewModel = Maximiz.ViewModels.Columns.Order;
using OrderModel = Maximiz.Database.Columns.Order;

namespace Maximiz.ViewModels.Columns.Translation
{

    /// <summary>
    /// Translates our order.
    /// </summary>
    internal static class OrderTranslator
    {

        /// <summary>
        /// Translates from <see cref="OrderViewModel"/> to <see cref="OrderModel"/>.
        /// </summary>
        /// <param name="order"><see cref="OrderViewModel"/></param>
        /// <returns><see cref="OrderModel"/></returns>
        public static OrderModel Translate(OrderViewModel order)
        {
            switch (order)
            {
                case OrderViewModel.Ascending:
                    return OrderModel.Ascending;
                case OrderViewModel.Descending:
                    return OrderModel.Descending;
            }

            throw new System.InvalidOperationException(nameof(order));
        }

    }
}
