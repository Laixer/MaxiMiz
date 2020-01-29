using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Commiting
{

    /// <summary>
    /// Contract for linking different entities in the linking table.
    /// </summary>
    public interface ILinkingCommitter
    {

        Task CopyLinksFromCampaignGroupAsync(IEnumerable<Campaign> campaigns);

        Task LinkCampaignAdGroupAsync(Guid campaignId, Guid adGroupId);

        Task LinkCampaignGroupAdGroupAsync(Guid campaignGroupId, Guid adGroupId);

        Task UnlinkCampaignAdGroupAsync(Guid campaignId, Guid adGroupId);

        Task UnlinkCampaignGroupAdGroupAsync(Guid campaignGroupId, Guid adGroupId);


    }
}
