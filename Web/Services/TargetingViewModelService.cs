using Maximiz.Services.Abstraction;
using Maximiz.ViewModels.Enums;
using System.Collections.Generic;

namespace Maximiz.Services
{

    /// <summary>
    /// Provides location options to our view models.
    /// </summary>
    public sealed class TargetingViewModelService : ITargetingViewModelService
    {

        /// <summary>
        /// Provides us with all currently valid <see cref="Location"/> options.
        /// </summary>
        /// <returns>Collection of valid options</returns>
        public IEnumerable<Location> GetLocationOptions()
            => new List<Location>
            {
                Location.DE,
                Location.ES,
                Location.FR,
                Location.NL,
                Location.UK
            };

        /// <summary>
        /// Provides us with all currently valid <see cref="Device"/> options.
        /// </summary>
        /// <returns>Collection of valid options</returns>
        public IEnumerable<Device> GetDeviceOptions()
            => new List<Device>
            {
                Device.Desktop,
                Device.Laptop,
                Device.Mobile,
                Device.Tablet,
                Device.Wearable
            };

        /// <summary>
        /// Provides us with all currently valid <see cref="OS"/> options.
        /// </summary>
        /// <returns>Collection of valid options</returns>
        public IEnumerable<OS> GetOperatingSystemOptions()
            => new List<OS> {
                OS.Android,
                OS.iOS,
                OS.Windows,
                OS.Chromeos,
                OS.Linux,
                OS.OSX,
                OS.Unix
            };
    }
}
