using Maximiz.Model.Enums;
using Poller.Taboola.Model;
using System;
using System.Collections.Generic;
using System.Linq;

using CampaignInternal = Maximiz.Model.Entity.Campaign;
using CampaignExternal = Poller.Taboola.Model.Campaign;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Used to convert certain target types to their corresponding target
    /// objects and vice versa. This gets called from the <see cref="MapperCampaign"/>.
    /// TODO Clean this up.
    /// </summary>
    internal partial class MapperTarget
    {

        /// <summary>
        /// Does all target conversion for a campaign.
        /// </summary>
        /// <param name="campaignInternal">The internal campaign after conversion</param>
        /// <param name="campaignExternal">The external campaign as conversion source</param>
        /// <returns>The same internal campaign but with mapped targets</returns>
        public CampaignInternal MapAllTargeting(CampaignInternal campaignInternal,
            CampaignExternal campaignExternal)
        {
            // Convert all target objects to the correct format
            // ConvertTarget(campaignExternal);

            // Map the locations
            var locations = TargetToLocationInts(campaignExternal.CountryTargeting);
            campaignInternal.LocationInclude = locations.Item1;
            campaignInternal.LocationExclude = locations.Item2;

            // Map other used targeting
            campaignInternal.Devices = MapDevices(campaignExternal.PlatformTargeting);
            // campaignInternal.OperatingSystems = MapOperatingSystems(campaignExternal.OsTargeting);
            campaignInternal.ConnectionTypes = AllConnectionTypes();

            // Return result explicitly
            return campaignInternal;
        }

        /// <summary>
        /// Does all target conversion for a campaign.
        /// </summary>
        /// <remarks>
        /// Unused targeting is set to null, being:
        /// - <see cref="CampaignExternal.SubCountryTargeting"/>.
        /// - <see cref="CampaignExternal.PostalCodeTargeting"/>.
        /// - <see cref="CampaignExternal.ContextualTargeting"/>.
        /// - <see cref="CampaignExternal.PublisherTargeting"/>.
        /// - <see cref="CampaignExternal.ConnectionTypeTargeting"/>.
        /// </remarks>
        /// <param name="campaignExternal">The external campaign after conversion</param>
        /// <param name="campaignInternal">The internal campaign as conversion source</param>
        /// <returns>The same external campaign but with mapped targets</returns>
        public CampaignExternal MapAllTargeting(CampaignExternal campaignExternal,
            CampaignInternal campaignInternal)
        {
            // Map the locations
            campaignExternal.CountryTargeting = LocationIntsToTarget(
                campaignInternal.LocationInclude, campaignInternal.LocationExclude);

            // Map other used targeting
            campaignExternal.PlatformTargeting = MapDevices(campaignInternal.Devices);
            // campaignExternal.OsTargeting = MapOperatingSystems(campaignInternal.OperatingSystems);
            campaignExternal.ConnectionTypeTargeting = TargetAll() as TargetDefault;

            // Set unused to null to prevent errors
            NullifyUnusedTargets(campaignExternal);

            // Return result explicitly
            return campaignExternal;
        }

        /// <summary>
        /// Sets all unused targets for version 1 to null.
        /// TODO Remove
        /// </summary>
        /// <param name="campaignExternal">The external campaign object</param>
        private void NullifyUnusedTargets(CampaignExternal campaignExternal)
        {
            campaignExternal.SubCountryTargeting = null;
            campaignExternal.PostalCodeTargeting = null;
            campaignExternal.ContextualTargeting = null;
            campaignExternal.PublisherTargeting = null;
            campaignExternal.ConnectionTypeTargeting = null;
        }

        /// <summary>
        /// Converts our location include and exclude integer arrays to a valid target object.
        /// </summary>
        /// <remarks>
        /// This returns an include all target object if we fail to convert
        /// any of the locations.
        /// </remarks>
        /// <param name="locationIncludeInts">The includes</param>
        /// <param name="locationExcludeInts">The excludes</param>
        /// <returns>The target object</returns>
        public TargetDefault LocationIntsToTarget(IEnumerable<int> locationIncludeInts,
            IEnumerable<int> locationExcludeInts)
        {
            try
            {
                var locationsInclude = locationIncludeInts.Select(x => IntToLocation(x)).ToList();
                var locationsExclude = locationIncludeInts.Select(x => IntToLocation(x)).ToList();
                var type = DetermineType(locationsInclude, locationsExclude);

                // Format correctly for type ALL
                if (type == TargetType.All)
                {
                    return new TargetDefault
                    {
                        Type = type,
                        Value = null
                    };
                }

                // TODO This is unsafe for another enum in targettype!
                var values = new List<string>();
                foreach (var location in (type == TargetType.Include) ? locationsInclude : locationsExclude)
                {
                    values.Add(LocationToTaboolaString(location));
                }

                // Construct result and return
                return new TargetDefault
                {
                    Type = type,
                    Value = values.ToArray()
                };
            }

            // If we fail to convert we return an all target.
            catch (ArgumentException e)
            {
                // TODO Log
                return new TargetDefault
                {
                    Type = TargetType.All,
                    Value = null
                };
            }

        }

        /// <summary>
        /// Converts our target object to the corresponding location include and exclude
        /// integer arrays.
        /// </summary>
        /// <remarks>
        /// In case of conversion failure this returns two empty arrays.
        /// </remarks>
        /// <param name="target">The target object</param>
        /// <returns>Locations include, locations exclude</returns>
        public (int[], int[]) TargetToLocationInts(TargetDefault target)
        {
            try
            {
                // In the case of all
                if (target.Type == TargetType.All) { return (AllLocationInts(), new int[0]); }

                // Else construct int arrays
                var locationEnums = target.Value.Select(x => TaboolaStringToLocation(x)).ToList();
                var locationInts = locationEnums.Select(x => LocationToInt(x)).ToArray();

                if (target.Type == TargetType.Include) { return (locationInts, new int[0]); }
                else { return (new int[0], locationInts); }
            }

            // Return default lists upon target conversion exception
            catch (ArgumentException e)
            {
                // TODO Log e
                return (new int[0], new int[0]);
            }
        }

        /// <summary>
        /// Determines our target type based on the include and exclude arrays.
        /// </summary>
        /// <param name="locationsInclude">The include locations</param>
        /// <param name="locationsExclude">The exclude locations</param>
        /// <returns></returns>
        private TargetType DetermineType(IEnumerable<Location> locationsInclude,
            IEnumerable<Location> locationsExclude)
        {
            // Null can occur, this is for safety
            var include = (locationsInclude ?? new List<Location>()).ToList();
            var exclude = (locationsExclude ?? new List<Location>()).ToList();

            // 2-dimensional switch statement construction
            if (include.Count == 0 && exclude.Count == 0) { return TargetType.All; }
            else if (include.Count > exclude.Count) { return TargetType.Include; }
            else if (include.Count < exclude.Count) { return TargetType.Exclude; }
            else
            {
                throw new ArgumentException(
             "Could not determine include or exclude type from locations.");
            }
        }

        /// <summary>
        /// Converts an integer from the location include or exclude integer
        /// array to the corresponding internal location model enum.
        /// </summary>
        /// <param name="integer">The used integer</param>
        /// <returns>The corresponding location enum</returns>
        private Location IntToLocation(int integer)
        {
            switch (integer)
            {
                case 0:
                    return Location.NL;
                case 1:
                    return Location.UK;
                case 2:
                    return Location.DE;
                case 3:
                    return Location.ES;
                case 4:
                    return Location.FR;
                default:
                    throw new ArgumentException($"Could not convert" +
                        $" location integer {integer} to location enum.");
            }
        }

        /// <summary>
        /// Converts a location enum to corresponding integer.
        /// </summary>
        /// <remarks>Throws an <see cref="ArgumentException"/> if we can't convert.</remarks>
        /// <param name="location">The location enum</param>
        /// <returns>The used integer</returns>
        private int LocationToInt(Location location)
        {
            switch (location)
            {
                case Location.NL:
                    return 0;
                case Location.UK:
                    return 1;
                case Location.DE:
                    return 2;
                case Location.ES:
                    return 3;
                case Location.FR:
                    return 4;
                default:
                    throw new ArgumentException($"Could not convert location" +
               $"{location} to corresponding internal used integer.");
            }
        }

        /// <summary>
        /// Converts a location object to a location string that Tabool accepts.
        /// </summary>
        /// <remarks>Throws if unable to convert</remarks>
        /// <param name="location">The location</param>
        /// <returns>The converted string</returns>
        private string LocationToTaboolaString(Location location)
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
                default:
                    throw new ArgumentException($"Could not convert location: {location.ToString()}");
            }
        }

        /// <summary>
        /// Converts a Taboola location string to an internal enum.
        /// </summary>
        /// <remarks>Throws an <see cref="ArgumentException"/> if we can't convert.</remarks>
        /// <param name="taboolaString">The Taboola string</param>
        /// <returns>The corresponding location enum</returns>
        private Location TaboolaStringToLocation(string taboolaString)
        {
            switch (taboolaString)
            {
                case "NL":
                    return Location.NL;
                case "UK":
                    return Location.UK;
                case "ES":
                    return Location.ES;
                case "DE":
                    return Location.DE;
                case "FR":
                    return Location.FR;
                default:
                    throw new ArgumentException($"Could not convert Taboola" +
                        $" location string {taboolaString} to internal location enum.");
            }
        }

        /// <summary>
        /// TODO Reconstruct! This is not bulletproof at all.
        /// </summary>
        /// <returns>An array representing all current known locations</returns>
        private int[] AllLocationInts()
        {
            return new[] { 0, 1, 2, 3, 4 };
        }

    }
}
