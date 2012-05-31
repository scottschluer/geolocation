using NUnit.Framework;

namespace Geolocation.UnitTests
{
    [TestFixture]
    public class CoordinateValidatorTests
    {
        [Test]
        public void LatitudeBelowMinimumFailsValidation()
        {
            Coordinate coordinate = Constants.Coordinates.LatitudeBelowMinimum;
         
            Assert.IsFalse(CoordinateValidator.Validate(coordinate.Latitude, coordinate.Longitude));
        }

        [Test]
        public void LatitudeAboveMaximumFailsValidation()
        {
            Coordinate coordinate = Constants.Coordinates.LatitudeAboveMaximum;

            Assert.IsFalse(CoordinateValidator.Validate(coordinate.Latitude, coordinate.Longitude));
        }

        [Test]
        public void LongitudeBelowMinimumFailsValidation()
        {
            Coordinate coordinate = Constants.Coordinates.LongitudeBelowMinumum;

            Assert.IsFalse(CoordinateValidator.Validate(coordinate.Latitude, coordinate.Longitude));
        }

        [Test]
        public void LongitudeAboveMaximumFailsValidation()
        {
            Coordinate coordinate = Constants.Coordinates.LongitudeAboveMaximum;

            Assert.IsFalse(CoordinateValidator.Validate(coordinate.Latitude, coordinate.Longitude));
        }

        [Test]
        public void LatitudeEqualToMinimumPassesValidation()
        {
            Coordinate coordinate = Constants.Coordinates.LatitudeEqualToMinimum;

            Assert.IsTrue(CoordinateValidator.Validate(coordinate.Latitude, coordinate.Longitude));
        }

        [Test]
        public void LatitudeEqualToMaximumPassesValidation()
        {
            Coordinate coordinate = Constants.Coordinates.LatitudeEqualToMaximum;

            Assert.IsTrue(CoordinateValidator.Validate(coordinate.Latitude, coordinate.Longitude));
        }

        [Test]
        public void LongitudeEqualToMinimumPassesValidation()
        {
            Coordinate coordinate = Constants.Coordinates.LongitudeEqualToMinimum;

            Assert.IsTrue(CoordinateValidator.Validate(coordinate.Latitude, coordinate.Longitude));
        }

        [Test]
        public void LongitudeEqualToMaximumPassesValidation()
        {
            Coordinate coordinate = Constants.Coordinates.LongitudeEqualToMaximum;

            Assert.IsTrue(CoordinateValidator.Validate(coordinate.Latitude, coordinate.Longitude));
        }

        [Test]
        public void ValidCoordinatePassesValidation()
        {
            Coordinate coordinate = Constants.Coordinates.ValidCoordinate;

            Assert.IsTrue(CoordinateValidator.Validate(coordinate.Latitude, coordinate.Longitude));
        }
    }
}
