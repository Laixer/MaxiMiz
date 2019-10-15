using Laixer.Library.Injection.Database;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Transactions.Creation
{

    internal class EntityCreator : ICreator<Entity>
    {

        private CampaignGroupCreator _campaignGroupCreator;
        private CampaignCreator _campaignCreator;
        private AdGroupCreator _adGroupCreator;
        private AdItemCreator _adItemCreator;

        public EntityCreator(IDatabaseProvider databaseProvider)
        {
            _campaignGroupCreator = new CampaignGroupCreator(databaseProvider);
            //_campaignCreator = new CampaignCreator(databaseProvider);
            //_adGroupCreator = new AdGroupCreator(databaseProvider);
            //_adItemCreator = new AdItemCreator(databaseProvider);
        }

        public Task<Entity> Create(Entity entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Entity>> CreateAll(IEnumerable<Entity> entities)
        {
            throw new NotImplementedException();
        }

    }
}
