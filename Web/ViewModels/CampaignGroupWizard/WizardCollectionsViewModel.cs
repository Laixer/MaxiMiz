using Maximiz.ViewModels.EntityModels;
using System;
using System.Collections.Generic;

namespace Maximiz.ViewModels.CampaignGroupWizard
{

    /// <summary>
    /// Contains all we need to start the campaign group wizard.
    /// </summary>
    public sealed class WizardCollectionsViewModel
    {

        /// <summary>
        /// Contains all accounts we can select.
        /// </summary>
        public IEnumerable<AccountModel> AccountsTaboolaPublisher { get; set; }

    }
}
