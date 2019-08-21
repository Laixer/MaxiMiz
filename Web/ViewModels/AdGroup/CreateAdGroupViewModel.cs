using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.ViewModels.AdGroup
{
    public class CreateAdGroupViewModel
    {
        /// <summary>
        /// Name of the Ad Group
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Destination URL
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// Tracking Code
        /// </summary>
        [Display(Name = "Tracking Code")]
        public string UTM { get; set; }
        public List<string> Titles { get; set; }
    }
}
