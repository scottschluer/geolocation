namespace Geolocation.UnitTests
{
    public static class Constants
    {
        public static class Coordinates
        {
            public static Coordinate ValidCoordinate
            {
                get { return new Coordinate { Latitude = 34.0675918, Longitude = -118.3977091 }; }
            }

            public static Coordinate ValidDestinationCoordinate
            {
                get { return new Coordinate { Latitude = 35.076234, Longitude = -118.9078687 }; }
            }

            public static Coordinate LatitudeBelowMinimum
            {
                get { return new Coordinate { Latitude = -91.0675918, Longitude = -118.3977091 }; }
            }

            public static Coordinate LatitudeAboveMaximum
            {
                get { return new Coordinate { Latitude = 97.0675918, Longitude = -118.3977091 }; }
            }

            public static Coordinate LongitudeBelowMinumum
            {
                get { return new Coordinate { Latitude = 34.0675918, Longitude = -197.3977091 }; }
            }

            public static Coordinate LongitudeAboveMaximum
            {
                get { return new Coordinate { Latitude = 34.0675918, Longitude = -187.3977091 }; }
            }

            public static Coordinate LatitudeEqualToMinimum
            {
                get { return new Coordinate { Latitude = -90.000, Longitude = -118.3977091 }; }
            }

            public static Coordinate LatitudeEqualToMaximum
            {
                get { return new Coordinate { Latitude = 90.0000, Longitude = -118.3977091 }; }
            }

            public static Coordinate LongitudeEqualToMinimum
            {
                get { return new Coordinate { Latitude = 34.0675918, Longitude = -180.0000 }; }
            }

            public static Coordinate LongitudeEqualToMaximum
            {
                get { return new Coordinate { Latitude = 34.0675918, Longitude = 180.0000 }; }
            }
        }
    }
}
