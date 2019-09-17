using System;
using Maximiz.Model.Enums;

namespace Maximiz.Model.Entity
{
    /// <summary>
    /// Group of advertisement items.
    /// </summary>
    [Serializable]
    public class AdGroup : EntityAudit<int>
    {
        /// <summary>
        /// Group name.
        /// </summary>
        public string Name { get; set; }

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
