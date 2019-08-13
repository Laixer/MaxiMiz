using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

using AccountMaximiz = Maximiz.Model.Entity.Account;
using AccountTaboola = Poller.Taboola.Model.Account;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// This class contains functionality to bridge the
    /// gap between our own models and those of the
    /// Taboola API.
    /// </summary>
    [Obsolete]
    class DataMapper
    {

        private IMapper _mapperAccount;

        /// <summary>
        /// Constructor.
        /// </summary>
        DataMapper() => CreateMappers();

        /// <summary>
        /// Creates our custom mapping configurations.
        /// </summary>
        private void CreateMappers()
        {
            _mapperAccount = new ConfigAccount().CreateMapper();
        }
       
        AccountMaximiz TaboolaToMaximiz(AccountTaboola account)
        {
            return _mapperAccount.Map<AccountTaboola, AccountMaximiz>(account);
        }

        AccountTaboola MaximizToTaboola(AccountMaximiz account)
        {
            return null;
        }

    }
}
