using Dapper;
using Maximiz.Database;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Maximiz.Repositories
{
    /// <summary>
    ///  Repository layer for operations related to <see cref="AdItem"></see> data.
    /// </summary>
    internal class AdItemRepository : RepositoryBase, IAdItemRepository
    {

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="crudInternalWebClient"></param>
        public AdItemRepository(ICrudInternalWebClient crudInternalWebClient)
            : base(crudInternalWebClient) { }

        public Task<AdItemWithStats> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AdItemWithStats>> GetAll(int page)
        {
            throw new NotImplementedException();
        }

        public Task QueryOnLoad()
        {
            throw new NotImplementedException();
        }
    }
}
