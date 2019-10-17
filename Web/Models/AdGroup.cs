using Maximiz.Model.Mvc.Entity;
using System;

namespace Maximiz.Mvc.Models
{

    /// <summary>
    /// Represents an ad group along with some numeric data.
    /// </summary>
    public class AdGroup : EntityAudit<Guid>
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
