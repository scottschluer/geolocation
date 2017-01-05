using System;
using NUnit.Framework;

namespace Geolocation.UnitTests
{
    [TestFixture]
    public class GeoCalculatorTests
    {
        [Test]
        public void EarthRadiusInMilesIsSetCorrectly()
        {
            double radius = GeoCalculator.EarthRadiusInMiles;
            const double expectedResult = 3959.0;

            Assert.AreEqual(expectedResult, radius);
        }

        [Test]
        public void EarthRadiusInNauticalMilesIsSetCorrectly()
        {
            double radius = GeoCalculator.EarthRadiusInNauticalMiles;
            const double expectedResult = 3440;

            Assert.AreEqual(expectedResult, radius);
        }

        [Test]
        public void EarthRadiusInKilometersIsSetCorrectly()
        {
            double radius = GeoCalculator.EarthRadiusInKilometers;
            const double expectedResult = 6371.0;

            Assert.AreEqual(expectedResult, radius);
        }

        [Test]
        public void EarthRadiusInMetersIsSetCorrectly()
        {
            double radius = GeoCalculator.EarthRadiusInMeters;
            const double expectedResult = 6371000.0;

            Assert.AreEqual(expectedResult, radius);
        }

        [Test]
        public void GetDistanceThrowsArgumentExceptionWithInvalidOriginCoordinates()
        {
            Coordinate origin = Constants.Coordinates.LatitudeBelowMinimum;
            Coordinate destination = Constants.Coordinates.ValidCoordinate;

            var ex = Assert.Throws<ArgumentException>(() => GeoCalculator.GetDistance(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude, 1));
            Assert.AreEqual("Invalid origin coordinates supplied.", ex.Message);
        }

        [Test]
        public void GetDistanceThrowsArgumentExceptionWithInvalidDestinationCoordinates()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.LatitudeBelowMinimum;

            var ex = Assert.Throws<ArgumentException>(() => GeoCalculator.GetDistance(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude, 1));
            Assert.AreEqual("Invalid destination coordinates supplied.", ex.Message);
        }

        [Test]
        public void GetDistanceReturnsCorrectResultInMiles()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.ValidDestinationCoordinate;

            double distance = GeoCalculator.GetDistance(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude);
            const double expectedResult = 75.5;

            Assert.AreEqual(expectedResult, distance);
        }

        [Test]
        public void GetDistanceReturnsCorrectResultInNauticalMiles()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.ValidDestinationCoordinate;

            double distance = GeoCalculator.GetDistance(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude, distanceUnit: DistanceUnit.NauticalMiles);
            const double expectedResult = 65.6;

            Assert.AreEqual(expectedResult, distance);
        }

        [Test]
        public void GetDistanceReturnsCorrectResultInKilometers()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.ValidDestinationCoordinate;

            double distance = GeoCalculator.GetDistance(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude, distanceUnit: DistanceUnit.Kilometers);
            const double expectedResult = 121.5;

            Assert.AreEqual(expectedResult, distance);
        }

        [Test]
        public void GetDistanceReturnsCorrectResultInMeters()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.ValidDestinationCoordinate;

            double distance = GeoCalculator.GetDistance(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude, distanceUnit: DistanceUnit.Meters);
            const double expectedResult = 121493.3;

            Assert.AreEqual(expectedResult, distance);
        }

        [Test]
        public void GetDistanceWithCoordinateObjectReturnsCorrectResult()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.ValidDestinationCoordinate;

            double distance = GeoCalculator.GetDistance(origin, destination);
            const double expectedResult = 75.5;

            Assert.AreEqual(expectedResult, distance);
        }

        [Test]
        public void GetBearingThrowsArgumentExceptionWithInvalidOriginCoordinates()
        {
            Coordinate origin = Constants.Coordinates.LatitudeBelowMinimum;
            Coordinate destination = Constants.Coordinates.ValidCoordinate;

            var ex = Assert.Throws<ArgumentException>(() => GeoCalculator.GetBearing(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude));
            Assert.AreEqual("Invalid origin coordinates supplied.", ex.Message);
        }

        [Test]
        public void GetBearingThrowsArgumentExceptionWithInvalidDestinationCoordinates()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.LatitudeBelowMinimum;

            var ex = Assert.Throws<ArgumentException>(() => GeoCalculator.GetBearing(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude));
            Assert.AreEqual("Invalid destination coordinates supplied.", ex.Message);
        }

        [Test]
        public void GetBearingReturnsCorrectResult()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.ValidDestinationCoordinate;

            double distance = GeoCalculator.GetBearing(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude);
            const double expectedResult = 337.39007167195172;

            Assert.AreEqual(expectedResult, distance);
        }

        [Test]
        public void GetBearingWithCoordinateObjectReturnsCorrectResult()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.ValidDestinationCoordinate;

            double distance = GeoCalculator.GetBearing(origin, destination);
            const double expectedResult = 337.39007167195172;

            Assert.AreEqual(expectedResult, distance);
        }

        [Test]
        public void GetDirectionThrowsArgumentExceptionWithInvalidOriginCoordinates()
        {
            Coordinate origin = Constants.Coordinates.LatitudeBelowMinimum;
            Coordinate destination = Constants.Coordinates.ValidCoordinate;

            var ex = Assert.Throws<ArgumentException>(() => GeoCalculator.GetDirection(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude));
            Assert.AreEqual("Invalid origin coordinates supplied.", ex.Message);
        }

        [Test]
        public void GetDirectionThrowsArgumentExceptionWithInvalidDestinationCoordinates()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.LatitudeBelowMinimum;

            var ex = Assert.Throws<ArgumentException>(() => GeoCalculator.GetDirection(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude));
            Assert.AreEqual("Invalid destination coordinates supplied.", ex.Message);
        }

        [Test]
        public void GetDirectionReturnsCorrectNorthWestResult()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.ValidDestinationCoordinate;

            string direction = GeoCalculator.GetDirection(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude);
            const string expectedResult = "NW";

            Assert.AreEqual(expectedResult, direction);
        }

        [Test]
        public void GetDirectionWithCoordinateObjectReturnsCorrectNorthWestResult()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.ValidDestinationCoordinate;

            string direction = GeoCalculator.GetDirection(origin, destination);
            const string expectedResult = "NW";

            Assert.AreEqual(expectedResult, direction);
        }

        //TODO: Add unit tests for all cardinal directions including boundary values.
    }
}
