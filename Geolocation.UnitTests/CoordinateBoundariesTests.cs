using System;
using NUnit.Framework;

namespace Geolocation.UnitTests
{
    [TestFixture]
    public class CoordinateBoundariesTests
    {
        [Test]
        public void ConstructorThrowsArgumentExceptionWithInvalidCoordinateParameters()
        {
            Coordinate origin = Constants.Coordinates.LatitudeBelowMinimum;
            int radius = 25;

            var ex = Assert.Throws<ArgumentException>(() => new CoordinateBoundaries(origin.Latitude, origin.Longitude, radius));
            Assert.AreEqual(ex.Message, "Invalid coordinates supplied.");
        }

        [Test]
        public void CalculateThrowsArgumentExceptionWithInvalidLatitudeProperty()
        {
            CoordinateBoundaries boundaries = new CoordinateBoundaries();
            
            var ex = Assert.Throws<ArgumentException>(() => boundaries.Latitude = Constants.Coordinates.LatitudeBelowMinimum.Latitude);

            Assert.AreEqual(ex.Message, "Invalid coordinates supplied.");
        }

        [Test]
        public void CalculateThrowsArgumentExceptionWithInvalidLongitudeProperty()
        {
            CoordinateBoundaries boundaries = new CoordinateBoundaries();

            var ex = Assert.Throws<ArgumentException>(() => boundaries.Longitude = Constants.Coordinates.LongitudeBelowMinumum.Longitude);

            Assert.AreEqual(ex.Message, "Invalid coordinates supplied.");
        }

        [Test]
        public void CalculateReturnsCorrectMinimumLatitude()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            int radius = 25;

            CoordinateBoundaries boundaries = new CoordinateBoundaries(origin.Latitude, origin.Longitude, radius);

            double expectedResult = 33.705272959420292;

            Assert.AreEqual(boundaries.MinLatitude, expectedResult);
        }

        [Test]
        public void CalculateReturnsCorrectMaximumLatitude()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            int radius = 25;

            CoordinateBoundaries boundaries = new CoordinateBoundaries(origin.Latitude, origin.Longitude, radius);

            double expectedResult = 34.429910640579713;

            Assert.AreEqual(boundaries.MaxLatitude, expectedResult);
        }

        [Test]
        public void CalculateReturnsCorrectMinimumLongitude()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            int radius = 25;

            CoordinateBoundaries boundaries = new CoordinateBoundaries(origin.Latitude, origin.Longitude, radius);

            double expectedResult = -118.83509292675051;

            Assert.AreEqual(boundaries.MinLongitude, expectedResult);
        }

        [Test]
        public void CalculateReturnsCorrectMaximumLongitude()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            int radius = 25;

            CoordinateBoundaries boundaries = new CoordinateBoundaries(origin.Latitude, origin.Longitude, radius);

            double expectedResult = -117.9603252732495;

            Assert.AreEqual(boundaries.MaxLongitude, expectedResult);
        }

        [Test]
        public void CalculateWithCoordinateObjectReturnsCorrectMinimumLatitude()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            int radius = 25;

            CoordinateBoundaries boundaries = new CoordinateBoundaries(origin, radius);

            double expectedResult = 33.705272959420292;

            Assert.AreEqual(boundaries.MinLatitude, expectedResult);
        }

        [Test]
        public void CalculateWithCoordinateObjectReturnsCorrectMaximumLatitude()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            int radius = 25;

            CoordinateBoundaries boundaries = new CoordinateBoundaries(origin, radius);

            double expectedResult = 34.429910640579713;

            Assert.AreEqual(boundaries.MaxLatitude, expectedResult);
        }

        [Test]
        public void CalculateWithCoordinateObjectReturnsCorrectMinimumLongitude()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            int radius = 25;

            CoordinateBoundaries boundaries = new CoordinateBoundaries(origin, radius);

            double expectedResult = -118.83509292675051;

            Assert.AreEqual(boundaries.MinLongitude, expectedResult);
        }

        [Test]
        public void CalculateWithCoordinateObjectReturnsCorrectMaximumLongitude()
        {
            Coordinate origin = Constants.Coordinates.ValidCoordinate;
            int radius = 25;

            CoordinateBoundaries boundaries = new CoordinateBoundaries(origin, radius);

            double expectedResult = -117.9603252732495;

            Assert.AreEqual(boundaries.MaxLongitude, expectedResult);
        }
    }
}
