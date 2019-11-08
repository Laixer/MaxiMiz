
using Maximiz.Services.Abstraction;

namespace Maximiz.Services
{

    /// <summary>
    /// Service that belongs to all views where we need to display some currency.
    /// </summary>
    public class CurrencyViewModelService : ICurrencyViewModelService
    {

        /// <summary>
        /// Computes the currentlyused currency symbol.
        /// TODO Implement.
        /// </summary>
        /// <returns>The correct currency symbol</returns>
        public string GetGlobalCurrencySymbol()
        {
            return "€";
        }

    }
}
