using System;
using System.Collections.Generic;

namespace Maximiz.Model.Operations
{

    /// <summary>
    /// Contains all <see cref="Entity.Entity"/>s involved in an <see cref="Operation"/>.
    /// </summary>
    public sealed class InvolvedEntitiesMap
    {

        public IList<Guid> CampaignGroupIds { get; set; } = new List<Guid>();

        public IList<Guid> CampaignIds { get; set; } = new List<Guid>();

        public IList<Guid> AdGroupIds { get; set; } = new List<Guid>();

        public IList<Guid> AdItemIds { get; set; } = new List<Guid>();

    }
}
