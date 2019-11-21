using Maximiz.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maximiz.Mapper
{

    /// <summary>
    /// Maps our locations.
    /// TODO Revisit
    /// </summary>
    sealed class MapperLocations
    {

        public List<Location> MapToLocations(int[] locationIds)
        {
            if (locationIds == null) { return new List<Location>(); }
            return locationIds.Select(x => MapToLocation(x)).ToList();
        }

        public int[] MapFromLocations(IEnumerable<Location> locations)
        { 
            if (locations == null) { return new int[0]; }
            return locations.Select(x => MapFromLocation(x)).ToArray();
        }

        private Location MapToLocation(int locationId)
        {
            switch (locationId)
            {
                case 0:
                    return Location.NL;
                case 1:
                    return Location.UK;
                case 2:
                    return Location.FR;
                case 3:
                    return Location.DE;
                case 4:
                    return Location.FR;
            }

            throw new InvalidOperationException(nameof(locationId));
        }

        private int MapFromLocation(Location location)
        {
            switch (location)
            {
                case Location.NL:
                    return 0;
                case Location.UK:
                    return 1;
                case Location.ES:
                    return 2;
                case Location.DE:
                    return 3;
                case Location.FR:
                    return 4;
            }

            throw new InvalidOperationException(nameof(location));
        }

    }
}
