using Maximiz.Services;

namespace Maximiz.ViewModels.Utility
{

    /// <summary>
    /// Contains our currency symbol.
    /// TODO Redesign.
    /// </summary>
    public class IncludeCurrencySymbolViewModel
    {

        /// <summary>
        /// Currency symbol.
        /// TODO Redesign!
        /// </summary>
        public string CurrencySymbol { get => new CurrencyViewModelService().GetGlobalCurrencySymbol(); }

    }
}
