using Maximiz.Model.Enums;
using Poller.Taboola.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// This converts the non-location target objects in 2 directions.
    /// TODO Don't 3-partial a class. Clean this up.
    /// </summary>
    internal partial class MapperTarget
    {

        /// <summary>
        /// Converts a target object to a device array.
        /// TODO Clean up
        /// </summary>
        /// <remarks>Returns an array of all possible devices upon conversion error</remarks>
        /// <param name="target">The target object</param>
        /// <returns>The corresponding device array</returns>
        internal Device[] MapDevices(TargetDefault target)
        {
            try
            {
                var result = new List<Device>();
                foreach (var s in target.Value)
                {
                    result.AddRange(TaboolaStringToDevice(s).ToArray());
                }
                return RemoveDuplicates(result.ToArray());
            }

            // Upon conversion error
            // TODO Log
            catch (ArgumentException e) { return AllDevices(); }
        }

        /// <summary>
        /// Converts a device array to a target object.
        /// </summary>
        /// <remarks>
        /// This removes duplicates from the devices array.
        /// This returns an all-targeting object upon conversion error.
        /// </remarks>
        /// <param name="devices">The devices</param>
        /// <returns>The target object</returns>
        internal TargetDefault MapDevices(Device[] devices)
        {
            try
            {
                devices = RemoveDuplicates(devices);
                var strings = devices.Select(x => DeviceToTaboolaString(x)).ToArray();
                return new TargetDefault
                {
                    Type = TargetType.Include,
                    Value = strings
                };
            }

            catch (ArgumentException e) { return TargetAll() as TargetDefault; }
        }

        /// <summary>
        /// Converts a target object to an array of operating systems used in our
        /// internal database.
        /// </summary>
        /// <remarks>Returns all operating systems upon conversion error</remarks>
        /// <param name="target">The target</param>
        /// <returns>The operating system array</returns>
        internal OS[] MapOperatingSystems(TargetBase target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts an array of operating systems to a Taboola target object.
        /// </summary>
        /// <remarks>Returns an all-targeting object upon conversion error</remarks>
        /// <param name="operatingSystems">The operating systems</param>
        /// <returns>The target object</returns>
        internal TargetDefault MapOperatingSystems(OS[] operatingSystems)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a taboola API target string to a device enum.
        /// </summary>
        /// <remarks>
        /// Throws an <see cref="ArgumentException"/> on conversion error.
        /// DESK is converted to <see cref="Device.Desktop"/> and <see cref="Device.Laptop"/>.
        /// PHON is converted to <see cref="Device.Mobile"/> and <see cref="Device.Wearable"/>.
        /// </remarks>
        /// <param name="taboolaString">The Taboola API target string</param>
        /// <returns>The devices</returns>
        private Device[] TaboolaStringToDevice(string taboolaString)
        {
            switch (taboolaString)
            {
                case "DESK":
                    return new[] { Device.Desktop, Device.Laptop };
                case "PHON":
                    return new[] { Device.Mobile, Device.Wearable };
                case "TBLT":
                    return new[] { Device.Tablet };
                default:
                    throw new ArgumentException($"Could not convert Taboola target" +
                        $"string {taboolaString} to internal device enum.");
            }
        }

        /// <summary>
        /// Converts a device enum to a string the Taboola API understands.
        /// </summary>
        /// <remarks>
        /// Throws an <see cref="ArgumentException"/> on conversion error.
        /// <see cref="Device.Wearable"/> gets converted to PHON.
        /// <see cref="Device.Laptop"/> gets converted to DESK.
        /// </remarks>
        /// <param name="device">The device enum</param>
        /// <returns>The taboola string</returns>
        private string DeviceToTaboolaString(Device device)
        {
            switch (device)
            {
                case Device.Mobile:
                    return "PHON";
                case Device.Tablet:
                    return "TBLT";
                case Device.Laptop:
                    return "DESK";
                case Device.Desktop:
                    return "DESK";
                case Device.Wearable:
                    return "PHON";
                default:
                    throw new ArgumentException($"Could not convert device" +
                        $"{device} to external taboola string.");
            }
        }

        private OS TaboolaStringToOperatingSystem(string taboolaString)
        {
            throw new NotImplementedException();
        }

        private string OperatingSystemToTaboolaString(OS operatingSystem)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Represents a target object that targets all.
        /// </summary>
        /// <returns>The created object</returns>
        internal TargetBase TargetAll()
        {
            return new TargetDefault
            {
                Type = TargetType.All,
                Value = null
            };
        }

        /// <summary>
        /// Returns an array representing all possible devices.
        /// </summary>
        /// <returns></returns>
        private Device[] AllDevices()
        {
            return new[]
            {
                Device.Desktop,
                Device.Laptop,
                Device.Mobile,
                Device.Tablet,
                Device.Wearable
            };
        }

        /// <summary>
        /// Returns an array representing all possible connection types.
        /// </summary>
        /// <returns>The array</returns>
        private ConnectionType[] AllConnectionTypes()
        {
            return new[]
            {
                ConnectionType.Cable,
                ConnectionType.Cellular,
                ConnectionType.Wifi
            };
        }

        /// <summary>
        /// Removes all duplicates from a device array as a new array object.
        /// </summary>
        /// <param name="input">The device array</param>
        /// <returns>No duplicates</returns>
        private Device[] RemoveDuplicates(Device[] input) =>
            input.Distinct().ToArray();

    }
}
