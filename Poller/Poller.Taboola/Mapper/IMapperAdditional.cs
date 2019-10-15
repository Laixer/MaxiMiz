using System.Collections.Generic;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Appends the <see cref="IMapper{TExternal, TCore}"/> interface to allow
    /// us to convert and additional item. This is used to convert the results
    /// we get from the reports API, which only need to be converted in one 
    /// direction.
    /// </summary>
    interface IMapperAdditional<TExternal, TCore, TExternalAdditional> : IMapper<TExternal, TCore>
    {

        /// <summary>
        /// Converter from external object to our core.
        /// </summary>
        /// <param name="from">External object</param>
        /// <returns>Core object</returns>
        TCore ConvertAdditional(TExternalAdditional from);

        /// <summary>
        /// Convert a range of external objects.
        /// </summary>
        /// <param name="list">The list</param>
        /// <returns>The converted list</returns>
        IEnumerable<TCore> ConvertAllAdditional(IEnumerable<TExternalAdditional> list);

    }
}
