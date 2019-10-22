using Maximiz.Model.Entity;
using Maximiz.ViewModels.EntityModels;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Maximiz.Mapper
{

    /// <summary>
    /// Mapper between <see cref="CampaignGroupWithStats"/> and 
    /// <see cref="CampaignGroupModel"/>.
    /// </summary>
    internal sealed class MapperCampaignGroupWithStats
        : IMapper<CampaignGroupWithStats, CampaignGroupModel>
    {

        /// <summary>
        /// Our mapper object.
        /// </summary>
        private static readonly AutoMapper.Mapper mapper;

        /// <summary>
        /// Called once to set up mapping configuration.
        /// </summary>
        static MapperCampaignGroupWithStats()
        {
            var config = new MapperConfiguration(
                cfg => cfg
                .CreateMap<CampaignGroupWithStats, CampaignGroupModel>()
                .ReverseMap());
            mapper = new AutoMapper.Mapper(config);
        }

        /// <summary>
        /// Maps from <see cref="CampaignGroupModel"/> to <see cref="CampaignGroupWithStats"/>.
        /// </summary>
        /// <param name="from"><see cref="CampaignGroupModel"/></param>
        /// <returns><see cref="CampaignGroupWithStats"/></returns>
        public CampaignGroupWithStats Convert(CampaignGroupModel from)
        {
            var result = mapper.Map<CampaignGroupWithStats>(from);
            return result;
        }

        /// <summary>
        /// Maps from <see cref="CampaignGroupWithStats"/> to <see cref="CampaignGroupModel"/>.
        /// </summary>
        /// <param name="from"><see cref="CampaignGroupWithStats"/></param>
        /// <returns><see cref="CampaignGroupModel"/></returns>
        public CampaignGroupModel Convert(CampaignGroupWithStats from)
        {
            var result = mapper.Map<CampaignGroupModel>(from);
            return result;
        }

        /// <summary>
        /// Convert a list of <see cref="CampaignGroupModel"/>s.
        /// </summary>
        /// <param name="from">The list</param>
        /// <returns>The converted items</returns>
        public IEnumerable<CampaignGroupWithStats> ConvertAll(IEnumerable<CampaignGroupModel> from)
            => from.ToList().Select(x => Convert(x));

        /// <summary>
        /// Convert a list of <see cref="CampaignGroupWithStats"/>s.
        /// </summary>
        /// <param name="from">The list</param>
        /// <returns>The converted items</returns>
        public IEnumerable<CampaignGroupModel> ConvertAll(IEnumerable<CampaignGroupWithStats> from)
            => from.ToList().Select(x => Convert(x));

    }
}
