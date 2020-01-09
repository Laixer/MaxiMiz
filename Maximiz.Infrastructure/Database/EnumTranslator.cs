using Maximiz.Model.Enums;
using System;
using System.Collections.Generic;

namespace Maximiz.Infrastructure.Database
{

    /// <summary>
    /// Translates our enums.
    /// </summary>
    internal static class EnumTranslator
    {

        internal static string Translate(CampaignStatus campaignStatus)
        {
            switch (campaignStatus)
            {
                case CampaignStatus.Unknown:
                    return "unknown";
                case CampaignStatus.Running:
                    return "running";
                case CampaignStatus.Paused:
                    return "paused";
                case CampaignStatus.Depleted:
                    return "depleted";
                case CampaignStatus.DepletedMonthly:
                    return "depleted_monthly";
                case CampaignStatus.Expired:
                    return "expired";
                case CampaignStatus.Terminated:
                    return "terminated";
                case CampaignStatus.Frozen:
                    return "frozen";
                case CampaignStatus.PendingApproval:
                    return "pending_approval";
                case CampaignStatus.Rejected:
                    return "rejected";
                case CampaignStatus.PendingStartDate:
                    return "pending_start_date";
            }

            throw new InvalidOperationException(nameof(campaignStatus));

        }

    }
}
