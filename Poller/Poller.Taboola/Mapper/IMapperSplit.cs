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
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        ExternalB Convert(Core from);

        /// <summary>
        /// Converter from 
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
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

    }
}
