using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Transactions.Creation
{
    public class AdItemCreator : ICreator<AdItem>
    {
        public Task<AdItem> Create(AdItem entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AdItem>> CreateAll(IEnumerable<AdItem> entities)
        {
            throw new NotImplementedException();
        }
    }
}
