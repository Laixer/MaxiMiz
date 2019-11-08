using Maximiz.Database.Columns;
using Maximiz.Database.Querying;
using Maximiz.Model.Entity;

namespace Maximiz.Repositories.Abstraction
{

    /// <summary>
    /// Interface for a <see cref="CampaignGroupWithStats"/> repository, which is only 
    /// responsible for performing read operations.
    /// </summary>
    public interface ICampaignGroupRepository : IRepository<CampaignGroupWithStats, QueryCampaignGroupWithStats, ColumnCampaignGroupWithStats>
    {
        //
    }
}
