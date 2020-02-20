
namespace Maximiz.ViewModels.EntityModels
{

    /// <summary>
    /// Used as a base for all entity models used in the views.
    /// </summary>
    public class EntityModel<TPrimary> {

        /// <summary>
        /// Represents the id of the entity, directly copied from the external
        /// model used in the data store.
        /// </summary>
        public TPrimary Id { get; set; }

    }
}
