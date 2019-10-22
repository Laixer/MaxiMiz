using Maximiz.Model.Entity;
using Maximiz.ViewModels.EntityModels;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Maximiz.Mapper
{

    /// <summary>
    /// Mapper between <see cref="AdItemWithStats"/> and <see cref="AdItemModel"/>.
    /// TODO Doc
    /// </summary>
    internal sealed class MapperAdItemWithStats : IMapper<AdItemWithStats, AdItemModel>
    {

        /// <summary>
        /// Our mapper object.
        /// </summary>
        private static readonly AutoMapper.Mapper mapper;

        /// <summary>
        /// Called once to set up mapping configuration.
        /// </summary>
        static MapperAdItemWithStats()
        {
            var config = new MapperConfiguration(
                cfg => cfg
                .CreateMap<AdItemWithStats, AdItemModel>()
                .ReverseMap());
            mapper = new AutoMapper.Mapper(config);
        }

        /// <summary>
        /// Maps from <see cref="AdItemModel"/> to <see cref="AdItemWithStats"/>.
        /// </summary>
        /// <param name="from"><see cref="AdItemModel"/></param>
        /// <returns><see cref="AdItemWithStats"/></returns>
        public AdItemWithStats Convert(AdItemModel from)
        {
            var result = mapper.Map<AdItemWithStats>(from);
            return result;
        }

        /// <summary>
        /// Maps from <see cref="AdItemWithStats"/> to <see cref="AdItemModel"/>.
        /// </summary>
        /// <param name="from"><see cref="AdItemWithStats"/></param>
        /// <returns><see cref="AdItemModel"/></returns>
        public AdItemModel Convert(AdItemWithStats from)
        {
            var result = mapper.Map<AdItemModel>(from);
            return result;
        }

        public IEnumerable<AdItemWithStats> ConvertAll(IEnumerable<AdItemModel> from)
            => from.ToList().Select(x => Convert(x));

        public IEnumerable<AdItemModel> ConvertAll(IEnumerable<AdItemWithStats> from)
            => from.ToList().Select(x => Convert(x));

    }
}
