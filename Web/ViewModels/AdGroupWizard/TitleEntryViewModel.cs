
namespace Maximiz.ViewModels.AdGroupWizard
{

    /// <summary>
    /// Title entry view model for ad group wizard.
    /// </summary>
    public sealed class TitleEntryViewModel
    {

        /// <summary>
        /// The title if already present, else null.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// If true this will get a remove button next to it.
        /// </summary>
        public bool IsRemovable { get; set; }

    }
}
