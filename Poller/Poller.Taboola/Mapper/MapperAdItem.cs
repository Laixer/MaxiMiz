
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

        /// <summary>
        /// This converts a Taboola co result ad item 
        /// to our core ad item. This is the result we
        /// get when calling the ad items API. This only
        /// maps the available parameters.
        /// </summary>
        /// <param name="from">Taboola co result</param>
        /// <returns>Core ad item</returns>
        public AdItemCore Convert(AdItemCoResult from)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// This converts a Taboola ad item to our
        /// core ad item. This is the result we get 
        /// when we call the report API. This only
        /// maps the available parameters.
        /// </summary>
        /// <param name="external">The Taboola ad item</param>
        /// <returns>The core ad item</returns>
        public AdItemCore Convert(AdItemTaboola external)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// This converts a core item to a Taboola
        /// co result. This is the result we get 
        /// when calling the report API.
        /// </summary>
        /// <param name="from">Core ad item</param>
        /// <returns>Taboola co result ad item</returns>
        public AdItemCoResult Convert(AdItemCore from)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// This converts a core item to a Taboola
        /// ad item. This is the result we get when 
        /// calling the ad item API.
        /// </summary>
        /// <param name="core">Core ad item</param>
        /// <returns>Taboola ad item</returns>
        AdItemTaboola IMapper<AdItemTaboola, AdItemCore>.Convert(AdItemCore core)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// This merges the two types of ad items into
        /// one. TODO Do we need this?
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public AdItemCoResult Merge(AdItemTaboola from, AdItemCoResult to)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// TODO Do we need this?
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public AdItemTaboola Merge(AdItemCoResult from, AdItemTaboola to)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Add Taboola item values to a core ad item.
        /// </summary>
        /// <param name="core">The core ad item</param>
        /// <param name="from">The taboola ad item</param>
        /// <returns>Core ad item with additional values</returns>
        public AdItemCore AddOnto(AdItemCore core, AdItemTaboola from)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Add Taboola co result ad item values to a 
        /// core ad item.
        /// </summary>
        /// <param name="core">The core ad item</param>
        /// <param name="from">The taboola co result ad item</param>
        /// <returns>Core ad item with additional values</returns>
        public AdItemCore AddOnto(AdItemCore core, AdItemCoResult from)
        {
            throw new System.NotImplementedException();
        }
    }
}
