using Dapper;
using Maximiz.Database;
using Maximiz.InputModels;
using Maximiz.Model;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using Maximiz.ServiceBus;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Maximiz.Repositories
{
    /// <summary>
    /// Repository layer for operations related to <see cref="AdGroup"></see> data.
    /// </summary>
    internal class AdGroupRepository : RepositoryBase, IAdGroupRepository
    {

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="crudInternalWebClient"></param>
        public AdGroupRepository(ICrudInternalWebClient crudInternalWebClient)
            : base(crudInternalWebClient) { }

        public Task<AdGroupWithStats> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AdGroupWithStats>> GetAll(int page)
        {
            throw new NotImplementedException();
        }

        public Task QueryOnLoad()
        {
            throw new NotImplementedException();
        }
    }
}
