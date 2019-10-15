using Laixer.Library.Injection.Database;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Transactions.Creation
{

    /// <summary>
    /// Creates campaign groups in our database.
    /// </summary>
    internal class CampaignGroupCreator : ICreator<CampaignGroup>
    {

        /// <summary>
        /// Creates database connections for us.
        /// </summary>
        private readonly IDatabaseProvider _databaseProvider;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="databaseProvider"></param>
        public CampaignGroupCreator(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public async Task<CampaignGroup> Create(CampaignGroup entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CampaignGroup>> CreateAll(IEnumerable<CampaignGroup> entities)
        {
            var result = new List<CampaignGroup>();
            foreach (var entity in entities) { result.Add(await Create(entity)); }
            return result;
        }

    }
}
