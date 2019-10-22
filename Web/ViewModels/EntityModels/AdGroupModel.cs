
namespace Maximiz.ViewModels.EntityModels
{

    /// <summary>
    /// Represents an ad group along with some numeric data.
    /// </summary>
    public sealed class AdGroupModel : EntityModel
    {

        /// <summary>
        /// Group name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User description to describe this ad group.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Array of image links.
        /// </summary>
        public string[] ImageLinks { get; set; }

        /// <summary>
        /// Array of titles.
        /// </summary>
        public string[] Titles { get; set; }

    }
}
