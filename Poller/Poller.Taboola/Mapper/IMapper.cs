using System;
using System.Collections.Generic;
using System.Text;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Interface for mapping objects from Taboola
    /// to our Core.
    /// </summary>
    public interface IMapper<TExternal, TCore>
    {

        /// <summary>
        /// Converter from external to core.
        /// </summary>
        /// <param name="core">Core object</param>
        /// <returns>External object</returns>
        TExternal Convert(TCore core);

        /// <summary>
        /// Converter from core to external.
        /// </summary>
        /// <param name="external">External object</param>
        /// <returns>Core object</returns>
        TCore Convert(TExternal external);

        /// <summary>
        /// Convert a range from external to core.
        /// </summary>
        /// <param name="list">List of externals</param>
        /// <returns>List of cores</returns>
        IEnumerable<TCore> ConvertAll(
            IEnumerable<TExternal> list);

        /// <summary>
        /// Convert a range from core to external.
        /// </summary>
        /// <param name="list">List of cores</param>
        /// <returns>List of externals</returns>
        IEnumerable<TExternal> ConvertAll(
            IEnumerable<TCore> list);

    }
}
