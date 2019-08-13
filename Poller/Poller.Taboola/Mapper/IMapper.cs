using System;
using System.Collections.Generic;
using System.Text;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Interface for mapping objects from Taboola
    /// to our Core.
    /// </summary>
    interface IMapper<External, Core>
    {

        /// <summary>
        /// Converter from external to core.
        /// </summary>
        /// <param name="core">Core object</param>
        /// <returns>External object</returns>
        External Convert(Core core);

        /// <summary>
        /// Converter from core to external.
        /// </summary>
        /// <param name="external">External object</param>
        /// <returns>Core object</returns>
        Core Convert(External external);

    }
}
