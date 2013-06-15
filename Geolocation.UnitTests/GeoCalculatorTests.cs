using System;
using NUnit.Framework;

namespace Geolocation.UnitTests
{
    [TestFixture]
    public class GeoCalculatorTests
    {
        [Test]
        public void EarthRadiusIsSetCorrectly()
        {
            double radius = GeoCalculator.EarthRadiusInMiles;
            const double expectedResult = 3956.0;

            Assert.AreEqual(radius, expectedResult);
        }

        [Test]
        public void GetDistanceThrowsArgumentExceptionWithInvalidOriginCoordinates()
        {
            Coordinate origin = Constants.Coordinates.LatitudeBelowMinimum;
            Coordinate destination = Constants.Coordinates.ValidCoordinate;

            var ex = Assert.Throws<ArgumentException>(() => GeoCalculator.GetDistance(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude, 1));
            Assert.AreEqual(ex.Message, "Invalid origin coordinates supplied.");
        }

        [Test]
        public void GetDistanceThrowsArgumentExceptionWithInvalidDestinationCoordinates()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.LatitudeBelowMinimum;

            var ex = Assert.Throws<ArgumentException>(() => GeoCalculator.GetDistance(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude, 1));
            Assert.AreEqual(ex.Message, "Invalid destination coordinates supplied.");
        }

        [Test]
        public void GetDistanceReturnsCorrectResult()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.ValidDestinationCoordinate;

            double distance = GeoCalculator.GetDistance(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude, 1);
            const double expectedResult = 75.4;

            Assert.AreEqual(distance, expectedResult);
        }

        [Test]
        public void GetDistanceWithCoordinateObjectReturnsCorrectResult()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.ValidDestinationCoordinate;

            double distance = GeoCalculator.GetDistance(origin, destination, 1);
            const double expectedResult = 75.4;

            Assert.AreEqual(distance, expectedResult);
        }

        [Test]
        public void GetBearingThrowsArgumentExceptionWithInvalidOriginCoordinates()
        {
            Coordinate origin = Constants.Coordinates.LatitudeBelowMinimum;
            Coordinate destination = Constants.Coordinates.ValidCoordinate;

            var ex = Assert.Throws<ArgumentException>(() => GeoCalculator.GetBearing(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude));
            Assert.AreEqual(ex.Message, "Invalid origin coordinates supplied.");
        }

        [Test]
        public void GetBearingThrowsArgumentExceptionWithInvalidDestinationCoordinates()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.LatitudeBelowMinimum;

            var ex = Assert.Throws<ArgumentException>(() => GeoCalculator.GetBearing(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude));
            Assert.AreEqual(ex.Message, "Invalid destination coordinates supplied.");
        }

        [Test]
        public void GetBearingReturnsCorrectResult()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.ValidDestinationCoordinate;

            double distance = GeoCalculator.GetBearing(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude);
            const double expectedResult = 337.39007167195172;

            Assert.AreEqual(distance, expectedResult);
        }

        [Test]
        public void GetBearingWithCoordinateObjectReturnsCorrectResult()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.ValidDestinationCoordinate;

            double distance = GeoCalculator.GetBearing(origin, destination);
            const double expectedResult = 337.39007167195172;

            Assert.AreEqual(distance, expectedResult);
        }

        [Test]
        public void GetDirectionThrowsArgumentExceptionWithInvalidOriginCoordinates()
        {
            Coordinate origin = Constants.Coordinates.LatitudeBelowMinimum;
            Coordinate destination = Constants.Coordinates.ValidCoordinate;

            var ex = Assert.Throws<ArgumentException>(() => GeoCalculator.GetDirection(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude));
            Assert.AreEqual(ex.Message, "Invalid origin coordinates supplied.");
        }

        [Test]
        public void GetDirectionThrowsArgumentExceptionWithInvalidDestinationCoordinates()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.LatitudeBelowMinimum;

            var ex = Assert.Throws<ArgumentException>(() => GeoCalculator.GetDirection(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude));
            Assert.AreEqual(ex.Message, "Invalid destination coordinates supplied.");
        }

        [Test]
        public void GetDirectionReturnsCorrectNorthWestResult()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.ValidDestinationCoordinate;

            string direction = GeoCalculator.GetDirection(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude);
            const string expectedResult = "NW";

            Assert.AreEqual(direction, expectedResult);
        }

        [Test]
        public void GetDirectionWithCoordinateObjectReturnsCorrectNorthWestResult()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            Coordinate destination = Constants.Coordinates.ValidDestinationCoordinate;

            string direction = GeoCalculator.GetDirection(origin, destination);
            const string expectedResult = "NW";

            Assert.AreEqual(direction, expectedResult);
        }

        //TODO: Add unit tests for all cardinal directions including boundary values.
    }
}
