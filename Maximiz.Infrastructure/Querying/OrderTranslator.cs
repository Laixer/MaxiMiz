using Maximiz.Core.Querying;
using System;

namespace Maximiz.Infrastructure.Querying
{

    /// <summary>
    /// Used to translate everything <see cref="Order"/> related.
    /// </summary>
    internal static class OrderTranslator
    {

        /// <summary>
        /// Translate an <see cref="Order"/> to the corresponding SQL term.
        /// </summary>
        /// <param name="order"><see cref="Order"/></param>
        /// <returns>SQL term</returns>
        internal static string Translate(Order order)
        {
            switch (order)
            {
                case Order.Ascending:
                    return "ASC";
                case Order.Descending:
                    return "DESC";
            }

            throw new InvalidOperationException(nameof(order));
        }

    }
}
