using System;
using System.Collections.Generic;
using System.Text;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Interface used to split and merge objects.
    /// Some external objects are delivered to us
    /// in more than one way. This interface helps
    /// build a contract to handle the merging and
    /// splitting of these objects.
    /// </summary>
    interface IMapperSplit<TExternalA, TExternalB, TCore> : IMapper<TExternalA, TCore>
    {

        /// <summary>
        /// Convert from core to our external object.
        /// </summary>
        /// <param name="from">The core object</param>
        /// <returns>The external object</returns>
        TExternalB ConvertAdditional(TCore from);

        /// <summary>
        /// Converter from external object to our core.
        /// </summary>
        /// <param name="from">External object</param>
        /// <returns>Core object</returns>
        TCore ConvertAdditional(TExternalB from);

        /// <summary>
        /// Adds the parameters from one half of the
        /// split object to the other half.
        /// </summary>
        /// <param name="from">The object to copy from</param>
        /// <param name="to">The object to copy to</param>
        /// <returns>The merged to object</returns>
        TExternalB Merge(TExternalA from, TExternalB to);

        /// <summary>
        /// Adds the parameters from one half of the
        /// split object to the other half.
        /// </summary>
        /// <param name="from">The object to copy from</param>
        /// <param name="to">The object to copy to</param>
        /// <returns>The merged to object</returns>
        TExternalA Merge(TExternalB from, TExternalA to);

        /// <summary>
        /// Add the parameters of one half of the split
        /// object to a core object. The core object can
        /// already contain some values.
        /// </summary>
        /// <param name="core">The core object</param>
        /// <param name="from">External object</param>
        /// <returns></returns>
        TCore AddOnto(TCore core, TExternalA from);

        /// <summary>
        /// Add the parameters of one half of the split
        /// object to a core object. The core object can
        /// already contain some values.
        /// </summary>
        /// <param name="core">The core object</param>
        /// <param name="from">External object</param>
        /// <returns></returns>
        TCore AddOnto(TCore core, TExternalB from);

    }
}
