using Maximiz.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Abstraction
{

    /// <summary>
    /// Interface for a <see cref="CampaignWithStats"/> repository, which is only responsible
    /// for performing read operations.
    /// TODO Doc.
    /// </summary>
    public interface ICampaignRepository : IRepository<CampaignWithStats>
    {

        Task<IEnumerable<CampaignWithStats>> GetActive(int page);

        Task<IEnumerable<CampaignWithStats>> GetInactive(int page);

        Task<IEnumerable<CampaignWithStats>> GetPending(int page);

        Task<IEnumerable<CampaignWithStats>> GetHidden(int page);

        Task<IEnumerable<CampaignWithStats>> GetConcept(int page);

        Task<IEnumerable<CampaignWithStats>> GetDeleted(int page);

    }

}
