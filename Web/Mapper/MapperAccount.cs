using Maximiz.Model.Entity;
using Maximiz.ViewModels.EntityModels;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System;
using Maximiz.Model.Enums;
using Microsoft.Extensions.Logging;
using Poller.Helper;
using Maximiz.Mapper.AccountTypeDeserialization;
using Newtonsoft.Json.Linq;

namespace Maximiz.Mapper
{

    /// <summary>
    /// Mapper between <see cref="Account"/> and <see cref="AccountModel"/>.
    /// TODO Doc
    /// </summary>
    internal sealed class MapperAccount : IMapper<Account, AccountModel>
    {

        /// <summary>
        /// Our mapper object.
        /// TODO This should be a configuration and not a mapper (see AutoMapper docs).
        /// </summary>
        private static readonly AutoMapper.Mapper mapper;

        /// <summary>
        /// Loggin instance.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Called once to set up mapping configuration.
        /// </summary>
        static MapperAccount()
        {
            var config = new MapperConfiguration(
                cfg => cfg
                .CreateMap<Account, AccountModel>()
                .ReverseMap());
            mapper = new AutoMapper.Mapper(config);
        }

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public MapperAccount(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger(nameof(MapperAccount));
        }

        /// <summary>
        /// Maps from <see cref="AccountModel"/> to <see cref="Account"/>.
        /// </summary>
        /// <param name="from"><see cref="AccountModel"/></param>
        /// <returns><see cref="Account"/></returns>
        public Account Convert(AccountModel from)
        {
            var result = mapper.Map<Account>(from);
            return result;
        }

        /// <summary>
        /// Maps from <see cref="Account"/> to <see cref="AccountModel"/>.
        /// </summary>
        /// <param name="from"><see cref="Account"/></param>
        /// <returns><see cref="AccountModel"/></returns>
        public AccountModel Convert(Account from)
        {
            var result = mapper.Map<AccountModel>(from);
            result.AccountTypeString = MapAccountType(from);
            return result;
        }

        public IEnumerable<Account> ConvertAll(IEnumerable<AccountModel> from)
            => from.ToList().Select(x => Convert(x));

        public IEnumerable<AccountModel> ConvertAll(IEnumerable<Account> from)
            => from.ToList().Select(x => Convert(x));

        /// <summary>
        /// Maps our account type string.
        /// TODO This seems kind of a hacky fix. The problem is addressed at github issue 58,
        /// see https://github.com/Laixer/MaxiMiz/issues/58/>.
        /// </summary>
        /// <returns>The account type as a string</returns>
        private string MapAccountType(Account account)
        {
            switch (account.Publisher)
            {
                case Publisher.Taboola:
                    try
                    {
                        // TODO This seems a bit hacky
                        var details = Json.Deserialize<JObject>(account.Details);
                        if (details == null || !details.ContainsKey("partner_types")) { break; }
                        var partnerTypes = JArray.Parse(details["partner_types"].ToString());
                        if (partnerTypes.Count == 0) { break; }
                        return partnerTypes[0].ToString();
                    }
                    catch (Exception e)
                    {
                        logger.LogError($"Could not extract Taboola account type from details: {account.Details}. Message: {e.Message}");
                    }
                    //throw new InvalidOperationException(nameof(Account.Details)); // TODO Is this correct?
                    break;
            }

            return "Unknown account type";
            //throw new InvalidOperationException(nameof(Account.Publisher));
        }

    }
}
