using Maximiz.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Translation
{

    /// <summary>
    /// Contains functionality to convert a viewmodel enum to the corresponding
    /// string value for UI display purposes.
    /// </summary>
    public static class EnumTranslator
    {

        /// <summary>
        /// Translates a <see cref="BidStrategy"/> to a string.
        /// </summary>
        /// <param name="bidStrategy"><see cref="BidStrategy"/></param>
        /// <returns>Corresponding string value</returns>
        public static string TranslateBidStrategy(BidStrategy bidStrategy)
        {
            switch (bidStrategy)
            {
                case BidStrategy.Smart:
                    return "Smart";
                case BidStrategy.Fixed:
                    return "Fixed";
            }

            throw new InvalidOperationException(nameof(bidStrategy));
        }

        /// <summary>
        /// Translates a <see cref="Delivery"/> to a string.
        /// </summary>
        /// <param name="delivery"><see cref="Delivery"/></param>
        /// <returns>Corresponding string value</returns>
        public static string TranslateDelivery(Delivery delivery)
        {
            switch (delivery)
            {
                case Delivery.Balanced:
                    return "Balanced";
                case Delivery.Accelerated:
                    return "Accelerated";
                case Delivery.Strict:
                    return "Strict";
            }

            throw new InvalidOperationException(nameof(delivery));
        }

        /// <summary>
        /// Translates a <see cref="Publisher"/> to a string.
        /// </summary>
        /// <param name="publisher"><see cref="Publisher"/></param>
        /// <returns>Corresponding string value</returns>
        public static string TranslatePublisher(Publisher publisher)
        {
            switch (publisher)
            {
                case Publisher.Unknown:
                    return "Unknown";
                case Publisher.Google:
                    return "Google";
                case Publisher.Taboola:
                    return "Taboola";
                case Publisher.Outbrain:
                    return "Outbrain";
                case Publisher.Adroll:
                    return "Adroll";
                case Publisher.Criteo:
                    return "Criteo";
            }

            throw new InvalidOperationException(nameof(publisher));
        }

        /// <summary>
        /// Translates a <see cref="BudgetModel"/> to a string.
        /// </summary>
        /// <param name="budgetModel"><see cref="BudgetModel"/></param>
        /// <returns>Corresponding string value</returns>
        public static string TranslateBudgetModel(BudgetModel budgetModel)
        {
            switch (budgetModel)
            {
                case BudgetModel.Campaign:
                    return "Campaign";
                case BudgetModel.Monthly:
                    return "Monthly";
            }

            throw new InvalidOperationException(nameof(budgetModel));
        }

    }
}
