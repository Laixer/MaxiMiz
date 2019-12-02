using Maximiz.Model.Entity;

namespace Maximiz.Core.Validation
{

    /// <summary>
    /// Contract for validating a given model, which can be either internal or external.
    /// </summary>
    public interface IModelValidator<TEntity>
        where TEntity : Entity
    {

        /// <summary>
        /// Checks the validity for a given <see cref="TEntity"/>.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/></param>
        /// <returns>True if valid, false if not</returns>
        bool Validate(TEntity entity);

    }
}
