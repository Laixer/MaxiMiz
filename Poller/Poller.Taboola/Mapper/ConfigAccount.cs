using AutoMapper;
using Poller.Helper;
using Poller.Taboola.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AccountMaximiz = Maximiz.Model.Entity.Account;
using AccountTaboola = Poller.Taboola.Model.Account;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Creates a configuration for the account
    /// data type.
    /// </summary>
    [Obsolete]
    internal class ConfigAccount
    {

        private const string DEFAULT_PUBLISHER = "Taboola";
        private const string DEFAULT_NAME = "Default Name";

        /// <summary>
        /// Creates a mapper for accounts.
        /// TODO This ignores <see cref="Maximiz.Model.Entity.Entity{TPrimary}.Id"/>.
        /// </summary>
        /// <returns>The mapper</returns>
        internal IMapper CreateMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {

                // Custom conversion string to int
                // cfg.CreateMap<string, int>().ConvertUsing(s => Convert.ToInt32(s));

                cfg.CreateMap<AccountTaboola, AccountMaximiz>()

                // Main properties for AccountTaboola
                .ForMember(dest => dest.SecondaryId, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(DEFAULT_PUBLISHER))
                .ForMember(dest => dest.Name, opt =>
                {
                    opt.MapFrom(src => src.Name);
                    opt.NullSubstitute(DEFAULT_NAME);
                })
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))

                // Account details JSON object
                .ForMember(dest => dest.Details, opt => opt.MapFrom<TaboolaToJson>())

                // Ignore primary key
                .ForMember(dest => dest.Id, opt => opt.Ignore())
  
                // Also create reverse
                .ReverseMap();
            });

            configuration.AssertConfigurationIsValid();
            return configuration.CreateMapper();
        }

    }

    /// <summary>
    /// Converts from taboola account details 
    /// to json string.
    /// </summary>
    class TaboolaToJson : IValueResolver
        <AccountTaboola, AccountMaximiz, string>
    {
        public string Resolve(AccountTaboola source,
            AccountMaximiz destination,
            string member, ResolutionContext context)
        {
            return Json.Serialize(new AccountDetails
            {
                Id = source.Id,
                PartnerTypes = source.PartnerTypes?.Select(
                    s => s.ToLowerInvariant()).ToArray(),
                Type = source.Type.ToLower(),
                CampaignTypes = source.CampaignTypes?.Select(
                    s => s.ToLowerInvariant()).ToArray(),
            });
        }
    }

}
