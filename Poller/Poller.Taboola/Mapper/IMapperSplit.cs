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
    interface IMapperSplit<ExternalA, ExternalB, Core> : IMapper<ExternalA, Core>
    {

        /// <summary>
        /// Convert from core to our external object.
        /// </summary>
        /// <param name="from">The core object</param>
        /// <returns>The external object</returns>
        ExternalB Convert(Core from);

        /// <summary>
        /// Converter from external object to our core.
        /// </summary>
        /// <param name="from">External object</param>
        /// <returns>Core object</returns>
        Core Convert(ExternalB from);

        /// <summary>
        /// Adds the parameters from one half of the
        /// split object to the other half.
        /// </summary>
        /// <param name="from">The object to copy from</param>
        /// <param name="to">The object to copy to</param>
        /// <returns>The merged to object</returns>
        ExternalB Merge(ExternalA from, ExternalB to);

        /// <summary>
        /// Adds the parameters from one half of the
        /// split object to the other half.
        /// </summary>
        /// <param name="from">The object to copy from</param>
        /// <param name="to">The object to copy to</param>
        /// <returns>The merged to object</returns>
        ExternalA Merge(ExternalB from, ExternalA to);

        /// <summary>
        /// Add the parameters of one half of the split
        /// object to a core object. The core object can
        /// already contain some values.
        /// </summary>
        /// <param name="core">The core object</param>
        /// <param name="from">External object</param>
        /// <returns></returns>
        Core AddOnto(Core core, ExternalA from);

        /// <summary>
        /// Add the parameters of one half of the split
        /// object to a core object. The core object can
        /// already contain some values.
        /// </summary>
        /// <param name="core">The core object</param>
        /// <param name="from">External object</param>
        /// <returns></returns>
        Core AddOnto(Core core, ExternalB from);

    }
}
