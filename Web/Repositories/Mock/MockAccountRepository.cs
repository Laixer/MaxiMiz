using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using Maximiz.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Mock
{

    /// <summary>
    /// Mock implementation of an <see cref="Account"/> repository.
    /// </summary>
    internal sealed class MockAccountRepository : IAccountRepository
    {

        /// <summary>
        /// Gets all linked <see cref="AccountModel"/>s from our database.
        /// TODO How to manage visibility management etc? --> Identity?
        /// </summary>
        /// <returns>List of <see cref="AccountModel"/>s</returns>
        public Task<IEnumerable<Account>> GetAll()
            => Task.FromResult<IEnumerable<Account>>(new List<Account>
            {
                new Account {Name = "My first account", Id = Guid.NewGuid(), Publisher = Publisher.Taboola, Details = GetJsonAdvertiser() },
                new Account {Name = "My second account", Id = Guid.NewGuid(), Publisher = Publisher.Taboola, Details = GetJsonAdvertiser() },
                new Account {Name = "My third account", Id = Guid.NewGuid(), Publisher = Publisher.Taboola, Details = GetJsonAdvertiser() },
                new Account {Name = "My fourth account", Id = Guid.NewGuid(), Publisher = Publisher.Taboola, Details = GetJsonPublisher() },
            });

        private string GetJsonPublisher()
            => "{\"partner_types\": [ \"publisher\"],}";

        private string GetJsonAdvertiser()
            => "{\"partner_types\": [ \"advertiser\"],}";
    }
}
