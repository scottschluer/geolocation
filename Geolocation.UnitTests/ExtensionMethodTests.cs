using NUnit.Framework;

namespace Geolocation.UnitTests
{
    [TestFixture]
    public class ExtensionMethodTests
    {
        [Test]
        public void ToRadianReturnsCorrectResult()
        {
            Coordinate coordinate = Constants.Coordinates.ValidCoordinate;

            double result = coordinate.Latitude.ToRadian();
            const double expectedResult = 0.59459164513542162;

            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void DiffRadianReturnsCorrectResult()
        {
            Coordinate coordinate = Constants.Coordinates.ValidCoordinate;
            Coordinate destinationCoordinate = Constants.Coordinates.ValidDestinationCoordinate;

            double result = coordinate.Latitude.DiffRadian(destinationCoordinate.Latitude);
            const double expectedResult = 0.017604127364559075;

            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void ToBearingReturnsCorrectResult()
        {
            const double angleTangent = 1.8344799871616866;

            double result = angleTangent.ToBearing();
            const double expectedResult = 105.10796086557809;

            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void ToDegreesReturnsCorrectResult()
        {
            const double angleTangent = 1.8344799871616866;

            double result = angleTangent.ToDegrees();
            const double expectedResult = 105.10796086557809;

            Assert.AreEqual(result, expectedResult);
        }
    }
}
