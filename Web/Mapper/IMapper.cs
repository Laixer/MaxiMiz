using Maximiz.Model.Entity;
using Maximiz.ViewModels.EntityModels;
using System.Collections.Generic;

namespace Maximiz.Mapper
{

    /// <summary>
    /// Converts an <see cref="Entity"/> to the <see cref="EntityModel"/> that
    /// belongs to it. This exists to separate our internal models and our
    /// viewmodels. These two are NOT the same thing.
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TEntityModel">The entity model type</typeparam>
    public interface IMapper<TEntity, TEntityModel>
        where TEntity : Entity
        where TEntityModel : EntityModel
    {

        /// <summary>
        /// Converts an <see cref="Entity"/> to the <see cref="EntityModel"/>
        /// that belongs to it.
        /// </summary>
        /// <param name="from">The object from which to convert</param>
        /// <returns>The converted object</returns>
        TEntity Convert(TEntityModel from);

        /// <summary>
        /// Converts an <see cref="EntityModel"/> to the <see cref="Entity"/>
        /// that belongs to it.
        /// </summary>
        /// <param name="from">The object from which to convert</param>
        /// <returns>The converted object</returns>
        TEntityModel Convert(TEntity from);

        // TODO Doc
        IEnumerable<TEntity> ConvertAll(IEnumerable<TEntityModel> from);
        // TODO Doc
        IEnumerable<TEntityModel> ConvertAll(IEnumerable<TEntity> from);

    }
}
