using System;

namespace Maximiz.ViewModels.Utility
{
    public sealed class PageSelectorViewModel
    {

        /// <summary>
        /// Indicates our table type.
        /// </summary>
        public Enum TableType { get; set;}

        /// <summary>
        /// Indicates the selector id we use for this page selector.
        /// </summary>
        public string Identifier { get; set; }

    }
}
