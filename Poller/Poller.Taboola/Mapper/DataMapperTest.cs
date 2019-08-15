using System;
using System.Collections.Generic;
using System.Text;

using AccountCore = Maximiz.Model.Entity.Account;
using AccountTaboola = Poller.Taboola.Model.Account;
using CampaignCore = Maximiz.Model.Entity.Campaign;
using CampaignTaboola = Poller.Taboola.Model.Campaign;
using AdItemCore = Maximiz.Model.Entity.AdItem;
using AdItemTaboola = Poller.Taboola.Model.AdItem;
using AdItemCoResult = Poller.Taboola.Model.AdItemCoResult;

namespace Poller.Taboola.Mapper
{
    class DataMapperTest
    {

        private readonly MapperAccount _mapperAccount;
        private readonly MapperCampaign _mapperCampaign;
        private readonly MapperAdItem _mapperAdItem;

        public DataMapperTest()
        {
            _mapperAccount = new MapperAccount();
            _mapperCampaign = new MapperCampaign();
            _mapperAdItem = new MapperAdItem();
        }

        public void ProcessAccount(AccountCore input)
        {
            var taboola = _mapperAccount.Convert(input);
            var core = _mapperAccount.Convert(taboola);
            return;
        }

        public void ProcessCampaign(CampaignCore input)
        {
            var taboola = _mapperCampaign.Convert(input);
            var core = _mapperCampaign.Convert(taboola);
            return;
        }

        public void ProcessAdItem(AdItemTaboola input)
        {
            var taboola = _mapperAdItem.Convert(input);
            var core = _mapperAdItem.Convert(taboola);
            return;
        }

        public void ProcessAdItemCoResult(AdItemCoResult input)
        {
            var taboola = _mapperAdItem.Convert(input);
            var core = _mapperAdItem.Convert(taboola);
            return;
        }

    }
}
