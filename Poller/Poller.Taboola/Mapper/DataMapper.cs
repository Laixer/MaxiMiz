using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// This class contains functionality to bridge the
    /// gap between our own models and those of the
    /// Taboola API.
    /// </summary>
    public class DataMapper
    {

        private IMapper _mapperAccount;

        /// <summary>
        /// Constructor.
        /// </summary>
        public DataMapper() => CreateMappings();

        /// <summary>
        /// Creates our custom mapping configurations.
        /// </summary>
        private void CreateMappings()
        {
            CreateAccountMapping();
        }

        private void CreateAccountMapping()
        {
            var configuration = new MapperConfiguration(cfg =>
            {

                // Custom conversion string to int
                cfg.CreateMap<string, int>().ConvertUsing(s => Convert.ToInt32(s));

                // Map for account
                cfg.CreateMap<Taboola.Model.Account, Maximiz.Model.Entity.Account>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SecondaryId, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom("Taboola"))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.SecondaryId, opt => opt.MapFrom(src => src.Name))

                // Null substitution
                .ForMember(destination => destination.Name,
                opt => opt.NullSubstitute("Default name"))

                // Custom mapping


                // Also create reverse
                .ReverseMap();
            });

            configuration.AssertConfigurationIsValid();
            _mapperAccount = configuration.CreateMapper();
        }

        Maximiz.Model.Entity.Account TaboolaToMaximiz(Model.Account account)
        {
            return _mapperAccount.Map<Model.Account, Maximiz.Model.Entity.Account>(account);
        }

        void MaximizToTaboola()
        {
            //return _mapperAccount.
        }

        Maximiz.Model.Entity.Account TaboolaToMaximizHardCoded(Model.Account account)
        {
            var result = new Maximiz.Model.Entity.Account
            {


            };

            return result;
        }

    }
}
