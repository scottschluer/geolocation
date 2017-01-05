/* Geolocation Class Library
 * Author: Scott Schluer (scott.schluer@gmail.com)
 * May 29, 2012
 * https://github.com/scottschluer/Geolocation
 */

using System;

namespace Geolocation
{
    /// <summary>
    /// Various utility methods for calculating geographically-based values
    /// </summary>
    public static class GeoCalculator
    {
        public static double EarthRadiusInMiles = 3959.0;
        public static double EarthRadiusInNauticalMiles = 3440.0;
        public static double EarthRadiusInKilometers = 6371.0;
        public static double EarthRadiusInMeters = 6371000.0;

        /// <summary>   
        /// Calculate the distance between two sets of coordinates.
        /// <param name="originLatitude">The latitude of the origin location in decimal notation</param>
        /// <param name="originLongitude">The longitude of the origin location in decimal notation</param>
        /// <param name="destinationLatitude">The latitude of the destination location in decimal notation</param>
        /// <param name="destinationLongitude">The longitude of the destination location in decimal notation</param>
        /// <param name="decimalPlaces">The number of decimal places to round the return value to</param>
        /// <param name="distanceUnit">The unit of distance</param>
        /// <returns>A <see cref="Double"/> value representing the distance in miles from the origin to the destination coordinate</returns>
        /// </summary>
        public static double GetDistance(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude, int decimalPlaces = 1, DistanceUnit distanceUnit = DistanceUnit.Miles)
        {
            if (!CoordinateValidator.Validate(originLatitude, originLongitude))
                throw new ArgumentException("Invalid origin coordinates supplied.");
            if (!CoordinateValidator.Validate(destinationLatitude, destinationLongitude))
                throw new ArgumentException("Invalid destination coordinates supplied.");

            double radius = GetRadius(distanceUnit);
            return Math.Round(
                    radius * 2 *
                    Math.Asin(Math.Min(1,
                                       Math.Sqrt(
                                           (Math.Pow(Math.Sin(originLatitude.DiffRadian(destinationLatitude) / 2.0), 2.0) +
                                            Math.Cos(originLatitude.ToRadian()) * Math.Cos(destinationLatitude.ToRadian()) *
                                            Math.Pow(Math.Sin((originLongitude.DiffRadian(destinationLongitude)) / 2.0),
                                                     2.0))))), decimalPlaces);
        }
        
        /// <summary>
        /// Calculate the distance between two sets of <see cref="Coordinate"/> objects
        /// </summary>
        /// <param name="originCoordinate">A <see cref="Coordinate"/> object representing the origin location</param>
        /// <param name="destinationCoordinate">A <see cref="Coordinate"/> object representing the destination location</param>
        /// <param name="decimalPlaces">The number of decimal places to round the return value to</param>
        /// <param name="distanceUnit">The unit of distance</param>
        /// <returns>A <see cref="Double"/> value representing the distance in miles from the origin to the destination coordinate</returns>
        public static Double GetDistance(Coordinate originCoordinate, Coordinate destinationCoordinate, int decimalPlaces = 1, DistanceUnit distanceUnit = DistanceUnit.Miles)
        {
            return GetDistance(originCoordinate.Latitude, originCoordinate.Longitude, destinationCoordinate.Latitude,
                destinationCoordinate.Longitude, decimalPlaces, distanceUnit);
        }

        /// <summary>
        /// Calculates the bearing, in degrees between two geographic points
        /// </summary>
        /// <param name="originLatitude">The latitude of the origin location in decimal notation</param>
        /// <param name="originLongitude">The longitude of the origin location in decimal notation</param>
        /// <param name="destinationLatitude">The latitude of the destination location in decimal notation</param>
        /// <param name="destinationLongitude">The longitude of the destination location in decimal notation</param>
        /// <returns>A <see cref="Double"/> value indicating the bearing from the origin to the destination</returns>
        public static double GetBearing(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude)
        {
            if (!CoordinateValidator.Validate(originLatitude, originLongitude))
                throw new ArgumentException("Invalid origin coordinates supplied.");
            if (!CoordinateValidator.Validate(destinationLatitude, destinationLongitude))
                throw new ArgumentException("Invalid destination coordinates supplied.");

            var destinationRadian = (destinationLongitude - originLongitude).ToRadian();
            var destinationPhi = Math.Log(Math.Tan(destinationLatitude.ToRadian() / 2 + Math.PI / 4) / Math.Tan(originLatitude.ToRadian() / 2 + Math.PI / 4));

            if (Math.Abs(destinationRadian) > Math.PI)
                destinationRadian = destinationRadian > 0
                                        ? -(2 * Math.PI - destinationRadian)
                                        : (2 * Math.PI + destinationRadian);

            return Math.Atan2(destinationRadian, destinationPhi).ToBearing();
        }

        /// <summary>
        /// Calculates the bearing, in degrees between two <see cref="Coordinate"/> objects
        /// </summary>
        /// <param name="originCoordinate">A <see cref="Coordinate"/> object representing the origin location</param>
        /// <param name="destinationCoordinate">A <see cref="Coordinate"/> object representing the destination location</param>
        /// <returns>A <see cref="Double"/> value indicating the bearing from the origin to the destination</returns>
        public static double GetBearing(Coordinate originCoordinate, Coordinate destinationCoordinate)
        {
            return GetBearing(originCoordinate.Latitude, originCoordinate.Longitude, destinationCoordinate.Latitude,
                              destinationCoordinate.Longitude);
        }

        /// <summary>
        /// Gets the cardinal or ordinal direction from the origin point to the destination point
        /// </summary>
        /// <param name="originLatitude">The latitude of the origin location in decimal notation</param>
        /// <param name="originLongitude">The longitude of the origin location in decimal notation</param>
        /// <param name="destinationLatitude">The latitude of the destination location in decimal notation</param>
        /// <param name="destinationLongitude">The longitude of the destination location in decimal notation</param>
        /// <returns>A string value indicating the cardinal or ordinal direction from the origin to the desintation point</returns>
        public static string GetDirection(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude)
        {
            if (!CoordinateValidator.Validate(originLatitude, originLongitude))
                throw new ArgumentException("Invalid origin coordinates supplied.");
            if (!CoordinateValidator.Validate(destinationLatitude, destinationLongitude))
                throw new ArgumentException("Invalid destination coordinates supplied.");

            double bearing = GetBearing(originLatitude, originLongitude, destinationLatitude, destinationLongitude);

            if (bearing >= 337.5 || bearing <= 22.5) return "N";
            if (bearing > 22.5 && bearing <= 67.5) return "NE";
            if (bearing > 67.5 && bearing <= 112.5) return "E";
            if (bearing > 112.5 && bearing <= 157.5) return "SE";
            if (bearing > 157.5 && bearing <= 202.5) return "S";
            if (bearing > 202.5 && bearing <= 247.5) return "SW";
            if (bearing > 247.5 && bearing <= 292.5) return "W";
            if (bearing > 292.5 && bearing < 337.5) return "NW";

            return String.Empty;
        }

        /// <summary>
        /// Gets the cardinal or ordinal direction from the origin point to the destination point
        /// </summary>
        /// <param name="originCoordinate">A <see cref="Coordinate"/> object representing the origin location</param>
        /// <param name="destinationCoordinate">A <see cref="Coordinate"/> object representing the destination location</param>
        /// <returns>A <see cref="String"/> value indicating the cardinal or ordinal direction from the origin to the desintation point</returns>
        public static string GetDirection(Coordinate originCoordinate, Coordinate destinationCoordinate)
        {
            return GetDirection(originCoordinate.Latitude, originCoordinate.Longitude, destinationCoordinate.Latitude,
                                destinationCoordinate.Longitude);
        }

        private static double GetRadius(DistanceUnit distanceUnit)
        {
            switch (distanceUnit)
            {
                case DistanceUnit.Kilometers:
                    return EarthRadiusInKilometers;
                case DistanceUnit.Meters:
                    return EarthRadiusInMeters;
                case DistanceUnit.NauticalMiles:
                    return EarthRadiusInNauticalMiles;
                default:
                    return EarthRadiusInMiles;
            }
        }
    }
}
