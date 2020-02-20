using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maximiz.ViewModels.ImagePicker
{

    /// <summary>
    /// Viewmodel for posting a list of image <see cref="Uri"/>s.
    /// </summary>
    public sealed class ImageUrisViewModel
    {

        /// <summary>
        /// List of image <see cref="Uri"/>s.
        /// </summary>
        [Required]
        public IEnumerable<Uri> ImageUris { get; set; }

    }
}
