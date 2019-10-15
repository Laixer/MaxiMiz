using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Transactions.Creation
{
    public class AdGroupCreator : ICreator<AdGroup>
    {
        public Task<AdGroup> Create(AdGroup entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AdGroup>> CreateAll(IEnumerable<AdGroup> entities)
        {
            throw new NotImplementedException();
        }
    }
}
