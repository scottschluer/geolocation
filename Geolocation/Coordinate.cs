/* Geolocation Class Library
 * Author: Scott Schluer (scott.schluer@gmail.com)
 * May 29, 2012
 * https://github.com/scottschluer/Geolocation
 */

namespace Geolocation
{
    /// <summary>
    /// Coordinates, i.e. latitude and longitude.
    /// </summary>
    public struct Coordinate
    {
        /// <summary>
        /// Latitude.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Instantiate the object.
        /// </summary>
        /// <param name="latitude">Latitude.</param>
        /// <param name="longitude">Longitude.</param>
        public Coordinate(double latitude, double longitude) {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}
