using AutoMapper;
using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using Maximiz.ViewModels.EntityModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Poller.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maximiz.Mapper
{

    /// <summary>
    /// Mapper between <see cref="AccountWithStats"/> and <see cref="AccountModel"/>.
    /// </summary>
    internal sealed class MapperAccountWithStats : IMapper<AccountWithStats, AccountModel>
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
        static MapperAccountWithStats()
        {
            var config = new MapperConfiguration(
                cfg => cfg
                .CreateMap<AccountWithStats, AccountModel>()
                .ReverseMap());
            mapper = new AutoMapper.Mapper(config);
        }

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public MapperAccountWithStats(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger(nameof(MapperAccount));
        }

        /// <summary>
        /// Maps from <see cref="AccountModel"/> to <see cref="AccountWithStats"/>.
        /// </summary>
        /// <param name="from"><see cref="AccountModel"/></param>
        /// <returns><see cref="AccountWithStats"/></returns>
        public AccountWithStats Convert(AccountModel from)
        {
            var result = mapper.Map<AccountWithStats>(from);
            return result;
        }

        /// <summary>
        /// Maps from <see cref="AccountWithStats"/> to <see cref="AccountModel"/>.
        /// </summary>
        /// <param name="from"><see cref="AccountWithStats"/></param>
        /// <returns><see cref="AccountModel"/></returns>
        public AccountModel Convert(AccountWithStats from)
        {
            var result = mapper.Map<AccountModel>(from);
            result.AccountTypeString = MapAccountType(from);
            return result;
        }

        public IEnumerable<AccountWithStats> ConvertAll(IEnumerable<AccountModel> from)
            => from.ToList().Select(x => Convert(x));

        public IEnumerable<AccountModel> ConvertAll(IEnumerable<AccountWithStats> from)
            => from.ToList().Select(x => Convert(x));

        /// <summary>
        /// Maps our account type string.
        /// TODO This seems kind of a hacky fix. The problem is addressed at github issue 58,
        /// see https://github.com/Laixer/MaxiMiz/issues/58/>.
        /// </summary>
        /// <returns>The account type as a string</returns>
        private string MapAccountType(AccountWithStats account)
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
                    throw new InvalidOperationException(nameof(AccountWithStats.Details)); // TODO Is this correct?
            }

            // TODO How to handle?
            //throw new InvalidOperationException(nameof(AccountWithStats.Publisher));
            return "Unknown type";
        }

    }
}
