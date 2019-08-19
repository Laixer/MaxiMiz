using Poller.Extensions;
using Poller.Model.Data;
using Poller.Taboola.Model;
using System;
using System.Collections.Generic;
using System.Text;


namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// This class contains extensions for our 
    /// Taboola models. Converting enums to text
    /// is done by these functions.
    /// </summary>
    internal static class MapperExtensions
    {

        /// <summary>
        /// Converts our daily ad delivery model to the
        /// corresponding string.
        /// </summary>
        /// <param name="campaign">Base campaign</param>
        /// <returns>The converted string</returns>
        public static string GetDeliveryText(
            this Campaign campaign)
        {
            switch (campaign.DailyAdDeliveryModel)
            {
                case DailyAdDeliveryModel.Balanced:
                    return "balaned"; // TODO: This is a typo
                case DailyAdDeliveryModel.Accelerated:
                    return "accelerated";
                case DailyAdDeliveryModel.Strict:
                    return "strict";
            }

            throw new Exception("Could not convert daily ad " +
                "delivery model to corresponding string");
        }

        /// <summary>
        /// Converts our spending limit model to the
        /// corresponding string.
        /// </summary>
        /// <param name="campaign">Base campaign</param>
        /// <returns>The converted string</returns>
        public static string GetSpendingLimitModelText(
           this Campaign campaign)
        {
            switch (campaign.SpendingLimitModel)
            {
                case SpendingLimitModel.Monthly:
                    return "monthly";
                case SpendingLimitModel.Entire:
                    return "campaign";
            }

            throw new Exception("Could not convert spending" +
                "limit model to corresponding string");
        }

        /// <summary>
        /// Converts our approval state to string.
        /// </summary>
        /// <param name="adItem">Base campaign</param>
        /// <returns>Approval state as string</returns>
        public static string GetApprovalStateText(
            this Campaign campaign) =>
            campaign.ApprovalState.GetEnumMemberName();

        /// <summary>
        /// Converts our approval state to string.
        /// </summary>
        /// <param name="adItem">Base ad item</param>
        /// <returns>Approval state as string</returns>
        public static string GetApprovalStateText(
            this AdItem adItem) =>
             adItem.ApprovalState.GetEnumMemberName();

        /// <summary>
        /// Converts our title text to string. This handles
        /// any titles that are null or empty and changes
        /// them to "INVALID".
        /// </summary>
        /// <param name="adItem">Base ad item</param>
        /// <returns>The title text</returns>
        public static string GetTitleText(
            this AdItem adItem) =>
            string.IsNullOrEmpty(adItem.Title) ? 
            "INVALID" : adItem.Title;

    }
}
