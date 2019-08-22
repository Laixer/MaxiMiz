using Poller.Model.Data;
using Poller.Taboola.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Contains some utility functions for our
    /// data mappers to handle certain types of
    /// custom types.
    /// </summary>
    static internal class MapperUtility
    {

        /// <summary>
        /// Converts a campaign item status to an ad item 
        /// status.
        /// </summary>
        /// <param name="campaignItemStatus">In</param>
        /// <returns>Corresponding ad item status</returns>
        public static AdItemStatus FromCampaignItemStatus(
            CampaignItemStatus campaignItemStatus)
        {
            switch (campaignItemStatus)

            {
                case CampaignItemStatus.Running:
                    return AdItemStatus.Running;
                case CampaignItemStatus.Crawling:
                case CampaignItemStatus.CrawlingError:
                case CampaignItemStatus.NeedToEdit:
                case CampaignItemStatus.PendingApproval:
                    return AdItemStatus.Pending;
                case CampaignItemStatus.Paused:
                    return AdItemStatus.Paused;
                case CampaignItemStatus.Stopped:
                    return AdItemStatus.Stopped;
                case CampaignItemStatus.Rejected:
                    return AdItemStatus.Rejected;
                default:
                    return AdItemStatus.Unknown;
            }
        }

        /// <summary>
        /// Converts an ad item status to a campaign item
        /// status.
        /// </summary>
        /// <param name="adItemStatus">In</param>
        /// <returns>Corresponding campaign item status</returns>
        public static CampaignItemStatus FromAdItemStatus(
            AdItemStatus adItemStatus)
        {
            switch (adItemStatus)
            {
                case AdItemStatus.Running:
                    return CampaignItemStatus.Running;
                case AdItemStatus.Paused:
                    return CampaignItemStatus.Paused;
                case AdItemStatus.Stopped:
                    return CampaignItemStatus.Stopped;
                case AdItemStatus.Pending:
                    return CampaignItemStatus.PendingApproval;
                case AdItemStatus.Rejected:
                    return CampaignItemStatus.Rejected;
                default:
                    return CampaignItemStatus.PendingApproval;
            }
        }

    }
}
