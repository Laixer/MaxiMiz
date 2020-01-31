using System;

namespace Maximiz.QueryTranslation
{

    /// <summary>
    /// Maps our 
    /// </summary>
    internal sealed class OrderMapper
    {

        /// <summary>
        /// Maps a viewmodel <see cref="ViewModels.Columns.Order"/> to the
        /// equivalent core <see cref="Core.Querying.Order"/>.
        /// </summary>
        /// <param name="order"><see cref="ViewModels.Columns.Order"/></param>
        /// <returns><see cref="Core.Querying.Order"/></returns>
        internal Core.Querying.Order Map(ViewModels.Columns.Order order)
        {
            switch (order)
            {
                case ViewModels.Columns.Order.Ascending:
                    return Core.Querying.Order.Ascending;
                case ViewModels.Columns.Order.Descending:
                    return Core.Querying.Order.Descending;
            }

            throw new InvalidOperationException(nameof(order));
        }

    }
}
