/* Geolocation Class Library
 * Author: Scott Schluer (scott.schluer@gmail.com)
 * May 29, 2012
 * https://github.com/scottschluer/Geolocation
 */

namespace Geolocation
{
    public struct Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Coordinate(double latitude, double longitude) {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}
