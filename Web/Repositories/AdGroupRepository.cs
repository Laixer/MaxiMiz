using Maximiz.InputModels;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Repositories
{
    public class AdGroupRepository : IAdGroupRepository
    {
        public Task Create(AdGroup entity)
        {
            // ISQ

            // EWP

            // Done

            throw new NotImplementedException();
        }

        public Task CreateGroup(AdGroupInputModel inputModel)
        {
            // To do
            throw new NotImplementedException();
        }

        public Task Delete(AdGroup entity)
        {
            throw new NotImplementedException();
        }

        public Task<AdGroup> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(AdGroup entity)
        {
            throw new NotImplementedException();
        }
    }
}
