using Maximiz.Model.Entity;
using Maximiz.ViewModels.EntityModels;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

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
        /// </summary>
        private static readonly AutoMapper.Mapper mapper;

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
            return result;
        }

        public IEnumerable<Account> ConvertAll(IEnumerable<AccountModel> from)
            => from.ToList().Select(x => Convert(x));

        public IEnumerable<AccountModel> ConvertAll(IEnumerable<Account> from)
            => from.ToList().Select(x => Convert(x));

    }
}
