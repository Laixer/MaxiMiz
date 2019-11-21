using Maximiz.ViewModels.Enums;
using System;

namespace Maximiz.Translation
{

    /// <summary>
    /// Translates our <see cref="Location"/>s to a human readable format.
    /// </summary>
    public static class LocationTranslator
    {

        /// <summary>
        /// Converts a <see cref="Location"/> to a human readable string.
        /// </summary>
        /// <param name="location"><see cref="Location"/></param>
        /// <returns>Human readable string</returns>
        public static string TranslateLocation(Location location)
        {
            switch (location)
            {
                case Location.NL:
                    return "Netherlands";
                case Location.UK:
                    return "United Kingdom";
                case Location.ES:
                    return "Spain";
                case Location.DE:
                    return "Germany";
                case Location.FR:
                    return "France";
            }

            throw new InvalidOperationException(nameof(location));
        }

        /// <summary>
        /// Converts a <see cref="OS"/> to a human readable string.
        /// </summary>
        /// <param name="operatingSystem"><see cref="OS"/></param>
        /// <returns>Human readable string</returns>
        public static string TranslateOperatingSystem(OS operatingSystem)
        {
            switch (operatingSystem)
            {
                case OS.Windows:
                    return "Windows";
                case OS.Linux:
                    return "Linux";
                case OS.OSX:
                    return "Apple OSX";
                case OS.Android:
                    return "Android";
                case OS.iOS:
                    return "Apple iOS";
                case OS.Unix:
                    return "Unix";
                case OS.Chromeos:
                    return "Chrome OS";
            }

            throw new InvalidOperationException(nameof(operatingSystem));
        }

        /// <summary>
        /// Converts a <see cref="Device"/> to a human readable string.
        /// </summary>
        /// <param name="device"><see cref="Device"/></param>
        /// <returns>Human readable string</returns>
        public static string TranslateDevice(Device device)
        {
            switch (device)
            {
                case Device.Mobile:
                    return "Mobile";
                case Device.Tablet:
                    return "Tablet";
                case Device.Laptop:
                    return "Laptop";
                case Device.Desktop:
                    return "Desktop";
                case Device.Wearable:
                    return "Wearable";
            }

            throw new InvalidOperationException(nameof(device));
        }

    }
}
