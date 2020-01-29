using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Committing
{
    public sealed class AdItemCommitter : IAdItemCommitter
    {
        public Task<AdItem> CreateAsync(AdItem entity, CancellationToken token) => throw new NotImplementedException();

        public Task<AdItem> Delete(AdItem entity, CancellationToken token) => throw new NotImplementedException();

        public Task<AdItem> Update(AdItem entity, CancellationToken token) => throw new NotImplementedException();

        public Task<bool> UpdateBulkAsync(IEnumerable<AdItem> entities, CancellationToken token) => throw new NotImplementedException();
    }
}
