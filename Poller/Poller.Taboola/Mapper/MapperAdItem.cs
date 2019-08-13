
using Maximiz.Model.Entity;
using Poller.Taboola.Model;
using AdItemCore = Maximiz.Model.Entity.AdItem;
using AdItemTaboola = Poller.Taboola.Model.AdItem;
using AdItemCoResult = Poller.Taboola.Model.AdItemCoResult;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Our Ad Item mapper.
    /// </summary>
    class MapperAdItem : IMapperSplit<AdItemTaboola, AdItemCoResult, AdItemCore>
    {
        public AdItemCoResult Convert(AdItemCore from)
        {
            throw new System.NotImplementedException();
        }

        public AdItemCore Convert(AdItemCoResult from)
        {
            throw new System.NotImplementedException();
        }

        public AdItemCore Convert(AdItemTaboola external)
        {
            throw new System.NotImplementedException();
        }

        public AdItemCoResult Merge(AdItemTaboola from, AdItemCoResult to)
        {
            throw new System.NotImplementedException();
        }

        public AdItemTaboola Merge(AdItemCoResult from, AdItemTaboola to)
        {
            throw new System.NotImplementedException();
        }

        AdItemTaboola IMapper<AdItemTaboola, AdItemCore>.Convert(AdItemCore core)
        {
            throw new System.NotImplementedException();
        }
    }
}
