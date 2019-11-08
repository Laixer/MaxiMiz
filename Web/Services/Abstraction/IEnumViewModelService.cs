using Maximiz.ViewModels.Enums;
using System.Collections.Generic;

namespace Maximiz.Services.Abstraction
{

    /// <summary>
    /// Contract for handling view model enumerations.
    /// </summary>
    public interface IEnumViewModelService
    {

        /// <summary>
        /// Provides us with a list of currently valid <see cref="Delivery"/> enuums.
        /// </summary>
        /// <returns>The generated list</returns>
        IEnumerable<Delivery> GetDeliveryOptions();

        /// <summary>
        /// Provides us with a list of currently valid <see cref="BidStrategy"/> enuums.
        /// </summary>
        /// <returns>The generated list</returns>
        IEnumerable<BidStrategy> GetBidStrategyOptions();

        /// <summary>
        /// Provides us with a list of currently valid <see cref="Publisher"/> enums.
        /// </summary>
        /// <returns>The generated list</returns>
        IEnumerable<Publisher> GetPublisherOptions();

        /// <summary>
        /// Provides us with a list of currently valid <see cref="BudgetModel"/> enums.
        /// </summary>
        /// <returns>The generated list</returns>
        IEnumerable<BudgetModel> GetBudgetModelOptions();

    }
}
