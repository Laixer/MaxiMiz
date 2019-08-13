using System;
using System.Collections.Generic;
using System.Text;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Interface for mapping objects from Taboola
    /// to our Core.
    /// </summary>
    interface IMapper<A, B>
    {

        /// <summary>
        /// Converter from A to B.
        /// </summary>
        /// <param name="from">B object</param>
        /// <returns>A object</returns>
        A Convert(B from);

        /// <summary>
        /// Converter from B to A.
        /// </summary>
        /// <param name="from">A object</param>
        /// <returns>B object</returns>
        B Convert(A from);

    }
}
