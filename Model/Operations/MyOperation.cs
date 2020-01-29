using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;

namespace Maximiz.Model.Operations
{

    public sealed class MyOperation : EntityAudit<Guid>
    {

        public Entity<Guid> TopEntity { get; set; }

        public CrudAction CrudAction { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; }

        // TODO Can't I do this cleaner?
        public IList<AdGroupCampaignGroupLinkingEntry> AdGroupCampaignGroupLinksAdd { get; set; } = new List<AdGroupCampaignGroupLinkingEntry>();

        public IList<AdGroupCampaignGroupLinkingEntry> AdGroupCampaignGroupLinksRemove { get; set; } = new List<AdGroupCampaignGroupLinkingEntry>();

        public IList<AdGroupCampaignLinkingEntry> AdGroupCampaignLinksAdd { get; set; } = new List<AdGroupCampaignLinkingEntry>();

        public IList<AdGroupCampaignLinkingEntry> AdGroupCampaignLinksRemove { get; set; } = new List<AdGroupCampaignLinkingEntry>();

        // TODO ID Assignment of top entity x linking entries

    }

}
