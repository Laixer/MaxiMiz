using Maximiz.ViewModels.EntityModels;
using Maximiz.ViewModels.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maximiz.ViewModels.CampaignDetails
{

    /// <summary>
    /// Viewmodel used to display the details for a given <see cref="CampaignModel"/>.
    /// </summary>
    public sealed class CampaignDetailsViewModel
    {

        /// <summary>
        /// The campaign we are displaying.
        /// </summary>
        public CampaignModel Campaign { get; set; }

        /// <summary>
        /// The account to which theh campaign belongs.
        /// </summary>
        public AccountModel Account { get; set; }

    }
}
