using System;
using System.Collections.Generic;
using System.Text;

using AdItemCore = Maximiz.Model.Entity.AdItem;
using CampaignCore = Maximiz.Model.Entity.Campaign;
using AccountCore = Maximiz.Model.Entity.Account;
using Poller.Extensions;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Contains extensions for some core enum members.
    /// </summary>
    public static class MapperExtensionsCore
    {
        /// <param name="adItem">Core ad item</param>
        /// <returns>Status as text</returns>
        public static string GetStatusText(
            this AdItemCore adItem) =>
            adItem.Status.GetEnumMemberName();

        /// <param name="adItem">Core ad item</param>
        /// <returns>Approval state as text</returns>
        public static string GetApprovalStateText(
            this AdItemCore adItem) =>
            adItem.ApprovalState.GetEnumMemberName();

    }
}
