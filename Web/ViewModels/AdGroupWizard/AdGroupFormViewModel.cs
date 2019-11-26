using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maximiz.ViewModels.AdGroupWizard
{

    /// <summary>
    /// Contains all properties we want to assign to the form to create or
    /// modify an ad group.
    /// </summary>
    public sealed class AdGroupFormViewModel
    {

        /// <summary>
        /// The id of the ad group we are modifying. This can be null if it's a new creation.
        /// TODO Is this a bulletproof strategy?
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ad group name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Contains all our form titles.
        /// </summary>
        [Required]
        public IEnumerable<string> Titles { get; set; }

        /// <summary>
        /// Contains all links to our selected images.
        /// </summary>
        [Required]
        public IEnumerable<string> ImageLinks { get; set; }

    }
}
