using System;
using System.Collections.Generic;

namespace Maximiz.ViewModels.ImagePicker
{

    /// <summary>
    /// Viewmodel for displaying a list of images.
    /// </summary>
    public sealed class ImageListViewModel
    {

        /// <summary>
        /// List of image <see cref="Uri"/>s.
        /// </summary>
        public IEnumerable<Uri> ImageUris { get; set; }

    }

}
