using Maximiz.Model.Enums;
using System;
using System.Linq;

namespace Maximiz.Core.Utility
{

    /// <summary>
    /// Contains functionality to map from <see cref="Location"/> to the 
    /// corresponding integer value.
    /// </summary>
    public static class MapperLocationIntegers
    {

        /// <summary>
        /// Map from <see cref="Location"/> to the corresponding integer value.
        /// </summary>
        /// <param name="location"><see cref="Location"/></param>
        /// <returns>Corresponding integer value</returns>
        public static int Map(Location location)
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

        /// <summary>
        /// Map from location integer value to the corresponding <see cref="Location"/>.
        /// </summary>
        /// <param name="locationInteger">Location integer value</param>
        /// <returns><see cref="Location"/></returns>
        public static Location Map(int locationInteger)
        {
            switch (locationInteger)
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

            throw new InvalidOperationException(nameof(locationInteger));
        }

        public static int[] MapMultiple(Location[] locations)
            => locations.Select(x => Map(x)).ToArray();

        public static Location[] MapMultiple(int[] locationIntegers)
            => locationIntegers.Select(x => Map(x)).ToArray();

    }
}
