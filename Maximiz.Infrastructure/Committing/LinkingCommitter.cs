using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Committing
{
    public sealed class LinkingCommitter : ILinkingCommitter
    {

        /// <summary>
        /// Copies the <see cref="AdGroup"/> links of a collection of <see cref="Campaign"/>s 
        /// based on the links from their corresponding <see cref="CampaignGroup"/>.
        /// </summary>
        /// <param name="campaigns"><see cref="Campaign"/></param>
        /// <returns><see cref="Task"/></returns>
        public Task CopyLinksFromCampaignGroupAsync(IEnumerable<Campaign> campaigns)
        {
            throw new NotImplementedException();
        }

        public Task LinkCampaignAdGroupAsync(Guid campaignId, Guid adGroupId)
        {
            throw new NotImplementedException();
        }

        public Task LinkCampaignGroupAdGroupAsync(Guid campaignGroupId, Guid adGroupId)
        {
            throw new NotImplementedException();
        }

        public Task UnlinkCampaignAdGroupAsync(Guid campaignId, Guid adGroupId)
        {
            throw new NotImplementedException();
        }

        public Task UnlinkCampaignGroupAdGroupAsync(Guid campaignGroupId, Guid adGroupId)
        {
            throw new NotImplementedException();
        }

    }

}
