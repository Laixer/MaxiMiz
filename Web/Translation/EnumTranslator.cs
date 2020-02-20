using Maximiz.ViewModels.AdGroupOverview;
using Maximiz.ViewModels.CampaignOverview;
using Maximiz.ViewModels.Enums;
using System;

namespace Maximiz.Translation
{

    /// <summary>
    /// Contains functionality to convert a viewmodel enum to the corresponding
    /// string value for UI display purposes.
    /// </summary>
    public static partial class EnumTranslator
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
                    return "In total";
                case BudgetModel.Monthly:
                    return "Monthly";
            }

            throw new InvalidOperationException(nameof(budgetModel));
        }

        /// <summary>
        /// Translates an account type string to the corresponding string.
        /// TODO this seems too strongly typed.
        /// </summary>
        /// <remarks>
        /// This needs the publisher enum because we have to validate that
        /// our publisher-accounttype combination is valid.
        /// If it's invalid we just display nothing. TODO Should we?
        /// </remarks>
        /// <param name="accountTypeString">The account type string as 
        /// extracted from the database json details object 
        /// (TODO TMI for this entity?)</param>
        /// <param name="publisher"><see cref="Publisher"/></param>
        /// <returns>Account type as string</returns>
        public static string TranslateAccountType(Publisher publisher, string accountTypeString)
        {
            switch (publisher)
            {
                case Publisher.Taboola:
                    switch (accountTypeString)
                    {
                        case "publisher":
                            return "Publisher";
                        case "advertiser":
                            return "Advertiser";
                    }
                    return ""; // TODO Should we?
            }

            throw new InvalidOperationException(nameof(publisher));
        }

        /// <summary>
        /// Converts a <see cref="CampaignTableType"/> to its corresponding string value.
        /// </summary>
        /// <param name="table"><see cref="CampaignTableType"/></param>
        /// <returns>String value</returns>
        public static string TranslateCampaignTableType(CampaignTableType table)
        {
            switch (table)
            {
                case CampaignTableType.All:
                    return "All";
                case CampaignTableType.Active:
                    return "Active";
                case CampaignTableType.Inactive:
                    return "Inactive";
                case CampaignTableType.Pending:
                    return "Pending";
            }

            throw new InvalidOperationException(nameof(table));
        }

        /// <summary>
        /// Converts a <see cref="AdGroupOverviewTableType"/> to its corresponding string value.
        /// </summary>
        /// <param name="table"><see cref="AdGroupOverviewTableType"/></param>
        /// <returns>String value</returns>
        public static string TranslateAdGroupTableType(AdGroupOverviewTableType table)
        {
            switch (table)
            {
                case AdGroupOverviewTableType.All:
                    return "All";
            }

            throw new InvalidOperationException(nameof(table));
        }

    }
}
