using Dapper;
using Laixer.Library.Injection.Database;
using Maximiz.Database;
using Maximiz.Database.Querying;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Data;

using System.Threading.Tasks;

namespace Maximiz.Repositories
{

    /// <summary>
    /// Repository layer for operations related to <see cref="Campaign"/> data.
    /// TODO Fix ugly interface implementation
    /// </summary>
    internal class CampaignGroupRepository : RepositoryBase, ICampaignGroupRepository
    {

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="crudInternalWebClient"></param>
        public CampaignGroupRepository(ICrudInternalWebClient crudInternalWebClient)
            : base(crudInternalWebClient) { }

        public Task<CampaignGroupWithStats> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CampaignGroupWithStats>> GetAll(int page)
        {
            throw new NotImplementedException();
        }

        public Task QueryOnLoad()
        {
            throw new NotImplementedException();
        }
    }
}
