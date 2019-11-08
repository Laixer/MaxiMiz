using Maximiz.ViewModels.Enums;
using System.Collections.Generic;

namespace Maximiz.Services.Abstraction
{

    /// <summary>
    /// Contract for supplying lists of target options to a view model.
    /// </summary>
    public interface ITargetingViewModelService
    {

        /// <summary>
        /// Provides us with all currently valid <see cref="Location"/> options.
        /// </summary>
        /// <returns>Collection of valid options</returns>
        IEnumerable<Location> GetLocationOptions();

        /// <summary>
        /// Provides us with all currently valid <see cref="Device"/> options.
        /// </summary>
        /// <returns>Collection of valid options</returns>
        IEnumerable<Device> GetDeviceOptions();

        /// <summary>
        /// Provides us with all currently valid <see cref="OS"/> options.
        /// </summary>
        /// <returns>Collection of valid options</returns>
        IEnumerable<OS> GetOperatingSystemOptions();

    }
}
