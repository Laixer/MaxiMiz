using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using System;

namespace Maximiz.Core.Utility
{

    /// <summary>
    /// Contains functionality to generate names for entities.
    /// </summary>
    public static class EntityNameGenerator
    {

        /// <summary>
        /// Generates a name for a campaign based on its properties.
        /// </summary>
        /// <param name="campaignGroup"><see cref="CampaignGroup"/></param>
        /// <param name="location"><see cref="Location"/></param>
        /// <param name="device"><see cref="Device"/></param>
        /// <param name="operatingSystem"><see cref="OS"/></param>
        /// <returns></returns>
        public static string ForCampaignFromCampaignGroup(CampaignGroup campaignGroup, Location location, Device device, OS operatingSystem)
        {
            if (campaignGroup == null) { throw new ArgumentNullException(nameof(campaignGroup)); }
            if (string.IsNullOrEmpty(campaignGroup.Name)) { throw new ArgumentNullException(nameof(campaignGroup.Name)); }

            return $"{campaignGroup.Name}_{TranslateLocation(location)}_{TranslateDevice(device)}_{TranslateOperatingSystem(operatingSystem)}";
        }

        private static string TranslateLocation(Location location)
        {
            switch (location)
            {
                case Location.NL:
                    return "NL";
                case Location.UK:
                    return "UK";
                case Location.ES:
                    return "ES";
                case Location.DE:
                    return "DE";
                case Location.FR:
                    return "FR";
            }

            throw new InvalidOperationException(nameof(location));
        }

        private static string TranslateDevice(Device device)
        {
            switch (device)
            {
                case Device.Mobile:
                    return "MOBI";
                case Device.Tablet:
                    return "TABL";
                case Device.Laptop:
                    return "LAPT";
                case Device.Desktop:
                    return "DESK";
                case Device.Wearable:
                    return "WEAR";
            }

            throw new InvalidOperationException(nameof(device));
        }

        private static string TranslateOperatingSystem(OS operatingSystem)
        {
            switch (operatingSystem)
            {
                case OS.Windows:
                    return "WIN";
                case OS.Linux:
                    return "LIN";
                case OS.OSX:
                    return "OSX";
                case OS.Android:
                    return "AND";
                case OS.iOS:
                    return "IOS";
                case OS.Unix:
                    return "UNI";
                case OS.Chromeos:
                    return "CHR";
            }

            throw new InvalidOperationException(nameof(operatingSystem));
        }

    }
}
