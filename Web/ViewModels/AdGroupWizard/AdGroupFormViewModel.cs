using System.Collections.Generic;

namespace Maximiz.ViewModels.AdgroupWizard
{

    /// <summary>
    /// Contains all properties we want to assign to the form to create or
    /// modify an ad group.
    /// </summary>
    public sealed class AdGroupFormViewModel
    {

        /// <summary>
        /// Ad group name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Contains all our form titles.
        /// </summary>
        public IEnumerable<string> Titles { get; set; }

        /// <summary>
        /// Contains all links to our selected images.
        /// </summary>
        public IEnumerable<string> ImageLinks { get; set; }

    }
}
