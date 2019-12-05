using Maximiz.Model.Entity;

namespace Maximiz.Core.Validation
{

    /// <summary>
    /// TODO Move.
    /// Validator for <see cref="Campaign"/>s.
    /// </summary>
    public sealed class TemporaryCampaignValidator : IModelValidator<Campaign>
    {

        /// <summary>
        /// Validates an <see cref="Campaign"/>.
        /// This will always return <see cref="true"/>.
        /// </summary>
        /// <param name="entity"><see cref="Campaign"/></param>
        /// <returns><see cref="true"/></returns>
        public bool Validate(Campaign entity)
            => true;

    }
}
