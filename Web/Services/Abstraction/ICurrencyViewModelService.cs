
namespace Maximiz.Services.Abstraction
{

    /// <summary>
    /// Contract for everything currency related.
    /// </summary>
    public interface ICurrencyViewModelService
    {

        /// <summary>
        /// Computes the currentlyused currency symbol.
        /// TODO Implement.
        /// </summary>
        /// <returns>The correct currency symbol</returns>
        string GetGlobalCurrencySymbol();

    }
}
