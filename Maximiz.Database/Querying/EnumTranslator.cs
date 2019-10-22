using Maximiz.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maximiz.Database.Querying
{

    /// <summary>
    /// Translates enums to the database accepted versions. This only implements
    /// the few enum cases that we actually need.
    /// </summary>
    public class EnumTranslator
    {

        /// <summary>
        /// Translates a <see cref="CampaignStatus"/> to a corresponding sql
        /// string that can be used in a statement.
        /// </summary>
        /// <param name="campaignStatus">The status input</param>
        /// <returns>The corresponding "enum-string" in the database</returns>
        public string Translate(CampaignStatus campaignStatus)
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

            throw new NotImplementedException($"No translation for campaign status: {campaignStatus}"); 
        }

    }
}
