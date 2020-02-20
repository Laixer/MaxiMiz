using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maximiz.Model.Operations
{
    public abstract class AdGroupLinkingEntry
    {

        public Guid AdGroupId { get; set; }

        public Guid LinkedId { get; set; }

    }

    public sealed class AdGroupCampaignGroupLinkingEntry : AdGroupLinkingEntry { }

    public sealed class AdGroupCampaignLinkingEntry : AdGroupLinkingEntry { }

}
