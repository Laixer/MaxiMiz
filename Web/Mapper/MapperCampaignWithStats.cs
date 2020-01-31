using Maximiz.Model.Entity;
using Maximiz.ViewModels.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Maximiz.Core.Utility;

namespace Maximiz.Mapper
{

    /// <summary>
    /// Mapper between <see cref="CampaignWithStats"/> and <see cref="CampaignModel"/>.
    /// TODO Doc
    /// TODO Instantiate mapper with configuration! Dont reuse it.
    /// </summary>
    internal sealed class MapperCampaignWithStats 
        : IMapper<CampaignWithStats, CampaignModel>
    {

        /// <summary>
        /// Our mapper object.
        /// </summary>
        private static readonly AutoMapper.Mapper mapper;

        /// <summary>
        /// Called once to set up mapping configuration.
        /// </summary>
        static MapperCampaignWithStats()
        {
            var config = new MapperConfiguration(
                cfg => cfg
                .CreateMap<CampaignWithStats, CampaignModel>()
                .ReverseMap());
            mapper = new AutoMapper.Mapper(config);
        }

        /// <summary>
        /// Maps from <see cref="CampaignModel"/> to <see cref="CampaignWithStats"/>.
        /// </summary>
        /// <param name="from"><see cref="CampaignModel"/></param>
        /// <returns><see cref="CampaignWithStats"/></returns>
        public CampaignWithStats Convert(CampaignModel from)
        {
            var result = mapper.Map<CampaignWithStats>(from);
            // TODO Location mapping

            return result;
        }

        /// <summary>
        /// Maps from <see cref="CampaignWithStats"/> to <see cref="CampaignModel"/>.
        /// </summary>
        /// <param name="from"><see cref="CampaignWithStats"/></param>
        /// <returns><see cref="CampaignModel"/></returns>
        public CampaignModel Convert(CampaignWithStats from)
        {
            var result = mapper.Map<CampaignModel>(from);

            // Explicit location mapping
            // TODO Is this the way to go?
            result.LocationInclude = from.LocationInclude.Select(x => MapperEnum.Map(MapperLocationIntegers.Map(x)));
            result.LocationExclude = from.LocationExclude.Select(x => MapperEnum.Map(MapperLocationIntegers.Map(x)));

            return result;
        }

        public IEnumerable<CampaignWithStats> ConvertAll(IEnumerable<CampaignModel> from)
            => from.ToList().Select(x => Convert(x));

        public IEnumerable<CampaignModel> ConvertAll(IEnumerable<CampaignWithStats> from)
            => from.ToList().Select(x => Convert(x));

    }
}
