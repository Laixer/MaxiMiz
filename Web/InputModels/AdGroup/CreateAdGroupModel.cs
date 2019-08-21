using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.InputModels.AdGroup
{
    public class CreateAdGroupModel
    {
        /// <summary>
        /// Name of the Ad Group
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Describes this group of ads
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Destination URL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Tracking Code
        /// </summary>
        [Display(Name = "Tracking Code")]
        public string Utm { get; set; }
        public List<string> Titles { get; set; }
    }
}
