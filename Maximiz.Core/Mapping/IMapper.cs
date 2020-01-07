using Maximiz.Model.Entity;
using System.Collections.Generic;

namespace unusedMaximiz.Core.Mapping
{

    /// <summary>
    /// Contract for a data conversion object, to be used for DTO conversion
    /// in a given layer.
    /// </summary>
    /// <typeparam name="TInternal"><see cref="Entity"/> within said layer</typeparam>
    /// <typeparam name="TExternal"><see cref="Entity"/> outside of said layer</typeparam>
    public interface IMapper<TInternal, TExternal>
        where TInternal : Entity
        where TExternal : Entity
    {

        /// <summary>
        /// Converts a <see cref="TExternal"/> to a <see cref="TInternal"/> object.
        /// </summary>
        /// <param name="input"><see cref="TExternal"/></param>
        /// <returns><see cref="TInternal"/></returns>
        TInternal Convert(TExternal input);

        /// <summary>
        /// Converts a <see cref="TInternal"/> to a <see cref="TExternal"/> object.
        /// </summary>
        /// <param name="input"><see cref="TInternal"/></param>
        /// <returns><see cref="TExternal"/></returns>
        TExternal Convert(TInternal input);

        /// <summary>
        /// Converts a collection of <see cref="TExternal"/> objects to a 
        /// collection of <see cref="TInternal"/> objects.
        /// </summary>
        /// <param name="input"><see cref="IEnumerable{TExternal}"/></param>
        /// <returns><see cref="IEnumerable{TInternal}"/></returns>
        IEnumerable<TInternal> ConvertAll(IEnumerable<TExternal> input);

        /// <summary>
        /// Converts a collection of <see cref="TInternal"/> objects to a 
        /// collection of <see cref="TExternal"/> objects.
        /// </summary>
        /// <param name="input"><see cref="IEnumerable{TInternal}"/></param>
        /// <returns><see cref="IEnumerable{TExternal}"/></returns>
        IEnumerable<TExternal> ConvertAll(IEnumerable<TInternal> input);

    }
}
