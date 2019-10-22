using Maximiz.Model.Entity;
using Maximiz.ViewModels.EntityModels;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Maximiz.Mapper
{

    /// <summary>
    /// Mapper between <see cref="AdGroupWithStats"/> and <see cref="AdGroupModel"/>.
    /// TODO Doc
    /// </summary>
    internal sealed class MapperAdGroupWithStats : IMapper<AdGroupWithStats, AdGroupModel>
    {

        /// <summary>
        /// Our mapper object.
        /// </summary>
        private static readonly AutoMapper.Mapper mapper;

        /// <summary>
        /// Called once to set up mapping configuration.
        /// </summary>
        static MapperAdGroupWithStats()
        {
            var config = new MapperConfiguration(
                cfg => cfg
                .CreateMap<AdGroupWithStats, AdGroupModel>()
                .ReverseMap());
            mapper = new AutoMapper.Mapper(config);
        }

        /// <summary>
        /// Maps from <see cref="AdGroupModel"/> to <see cref="AdGroupWithStats"/>.
        /// </summary>
        /// <param name="from"><see cref="AdGroupModel"/></param>
        /// <returns><see cref="AdGroupWithStats"/></returns>
        public AdGroupWithStats Convert(AdGroupModel from)
        {
            var result = mapper.Map<AdGroupWithStats>(from);
            return result;
        }

        /// <summary>
        /// Maps from <see cref="AdGroupWithStats"/> to <see cref="AdGroupModel"/>.
        /// </summary>
        /// <param name="from"><see cref="AdGroupWithStats"/></param>
        /// <returns><see cref="AdGroupModel"/></returns>
        public AdGroupModel Convert(AdGroupWithStats from)
        {
            var result = mapper.Map<AdGroupModel>(from);
            return result;
        }

        public IEnumerable<AdGroupWithStats> ConvertAll(IEnumerable<AdGroupModel> from)
            => from.ToList().Select(x => Convert(x));

        public IEnumerable<AdGroupModel> ConvertAll(IEnumerable<AdGroupWithStats> from)
            => from.ToList().Select(x => Convert(x));

    }
}
