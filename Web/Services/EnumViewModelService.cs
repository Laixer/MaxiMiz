using System.Collections.Generic;
using Maximiz.Services.Abstraction;
using Maximiz.ViewModels.Enums;

namespace Maximiz.Services
{

    /// <summary>
    /// Provides our views with proper lists (etc) of our viewmodel enums.
    /// TODO Maybe use constant lists? These never change.
    /// </summary>
    public sealed class EnumViewModelService : IEnumViewModelService
    {

        /// <summary>
        /// Provides us with a list of currently valid <see cref="Delivery"/> enums.
        /// </summary>
        /// <returns>The generated list</returns>
        public IEnumerable<BidStrategy> GetBidStrategyOptions()
            => new List<BidStrategy> {
                BidStrategy.Fixed,
                BidStrategy.Smart
            };

        /// <summary>
        /// Provides us with a list of currently valid <see cref="BidStrategy"/> enums.
        /// </summary>
        /// <returns>The generated list</returns>
        public IEnumerable<Delivery> GetDeliveryOptions()
            => new List<Delivery> {
                Delivery.Accelerated,
                Delivery.Balanced,
                Delivery.Strict
            };

        /// <summary>
        /// Provides us with a list of currently valid <see cref="Publisher"/> enums.
        /// </summary>
        /// <returns>The generated list</returns>
        public IEnumerable<Publisher> GetPublisherOptions()
            => new List<Publisher>
            {
                Publisher.Taboola,
                Publisher.Google,
                Publisher.Outbrain
            };

        /// <summary>
        /// Provides us with a list of currently valid <see cref="BudgetModel"/> enums.
        /// </summary>
        /// <returns>The generated list</returns>
        public IEnumerable<BudgetModel> GetBudgetModelOptions()
            => new List<BudgetModel>
            {
                BudgetModel.Campaign,
                BudgetModel.Monthly
            };



    }
}
