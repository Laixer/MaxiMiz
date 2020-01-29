using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Committing
{
    public sealed class AdGroupCommitter : IAdGroupCommitter
    {
        public Task<AdGroup> CreateAsync(AdGroup entity, CancellationToken token) => throw new NotImplementedException();

        public Task<AdGroup> Delete(AdGroup entity, CancellationToken token) => throw new NotImplementedException();

        public Task<AdGroup> Update(AdGroup entity, CancellationToken token) => throw new NotImplementedException();
    }
}
