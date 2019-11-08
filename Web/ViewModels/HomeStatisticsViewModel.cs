
using Maximiz.ViewModels.Utility;

namespace Maximiz.ViewModels
{

    /// <summary>
    /// Contains our statistics data for the home page.
    /// TODO Make modular.
    /// </summary>
    public sealed class HomeStatisticsViewModel : IncludeCurrencySymbolViewModel
    {

        public decimal SpendToday { get; set; } = 1337;

        public decimal SpendYesterday { get; set; } = 2337;

        public decimal SpendThisMonth { get; set; } = 133700;

        public decimal SpendLastMonth { get; set; } = 12247;

        public decimal SpendTotalTaboola { get; set; } = 1000000;

    }
}
