using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Transactions.Creation
{
    public class CampaignCreator : ICreator<Campaign>
    {
        public Task<Campaign> Create(Campaign entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Campaign>> CreateAll(IEnumerable<Campaign> entities)
        {
            throw new NotImplementedException();
        }
    }
}
