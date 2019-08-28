using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maximiz.InputModels
{
    public class AdGroupInputModel
    {
        /// <summary>
        /// Group name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description for all items in the group.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Display URL for all items in the group.
        /// </summary>
        [Display(Name = "Destination URL")]
        public string Url { get; set; }

        /// <summary>
        /// Tracking code for all the items in the group.
        /// </summary>
        [Display(Name = "Tracking Code")]
        public string Utm { get; set; }

        /// <summary>
        /// Ad items to create for this group.
        /// </summary>
        public List<AdItemInputModel> AdItems;
    }
}
