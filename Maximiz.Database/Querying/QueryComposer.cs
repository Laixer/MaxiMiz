using Maximiz.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maximiz.Database.Querying
{


    /// <summary>
    /// Composes query string for us based on multiple values of certain enums.
    /// </summary>
    public class QueryComposer
    {

        /// <summary>
        /// Translates single enums for us.
        /// </summary>
        private EnumTranslator enumTranslator = new EnumTranslator();

        /// <summary>
        /// Composes an SQL formatted search query to match all situations 
        /// where <see cref="CampaignStatus"/> represents an inactive campaign.
        /// </summary>
        /// <returns>Composed query string</returns>
        public string ComposeCampaignStatusInactive()
        {
            var inactives = new List<CampaignStatus>();
            inactives.Add(CampaignStatus.Depleted);
            inactives.Add(CampaignStatus.DepletedMonthly);
            inactives.Add(CampaignStatus.Expired);
            inactives.Add(CampaignStatus.Frozen);
            inactives.Add(CampaignStatus.Paused);
            inactives.Add(CampaignStatus.PendingApproval);
            inactives.Add(CampaignStatus.PendingStartDate);
            inactives.Add(CampaignStatus.Rejected);
            inactives.Add(CampaignStatus.Terminated);
            inactives.Add(CampaignStatus.Unknown);

            string query = "";
            for (int i = 0; i < inactives.Count; i++)
            {
                query += enumTranslator.Translate(inactives[i]);
                if (i < inactives.Count - 1)
                {
                    query += " OR ";
                }
            }

            return query;
        }

    }
}
