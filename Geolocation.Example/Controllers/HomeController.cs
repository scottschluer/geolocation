using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Geolocation.Example.ViewModels;
using Geolocation.Example.Models;

namespace Geolocation.Example.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new HomeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HomeViewModel model)
        {
            double radius = Convert.ToDouble(model.SelectedRadius);

            // These coordinates would normally be retrieved from a geocoding API such as Google's or Bing's.
            // See https://github.com/scottschluer/Geocoder for an easy to use Geocoder API for Google.
            var originCoordinate = new Coordinate { Latitude = 34.076234, Longitude = -118.395314 };

            model.Results = GetResults(originCoordinate, radius, model.SelectedDistanceUnit);

            return View(model);
        }

        private IEnumerable<ResultModel> GetResults(Coordinate originCoordinate, double radius, DistanceUnit distanceUnit)
        {
            // Get the boundaries (min and max) latitude and longitude values. This forms a "square" around the origin coordinate
            // with each leg of the square exactly "X" miles from the origin, where X is the selected radius.
            var boundaries = new CoordinateBoundaries(originCoordinate.Latitude, originCoordinate.Longitude, radius, distanceUnit);

            // Select from all of the locations
            return Locations
                // Where the location's latitude is between the min and max latitude boundaries
                .Where(x => x.Latitude >= boundaries.MinLatitude && x.Latitude <= boundaries.MaxLatitude)
                // And where the location's longitude is between the min and max longitude boundaries
                .Where(x => x.Longitude >= boundaries.MinLongitude && x.Longitude <= boundaries.MaxLongitude)
                // Populate an instance of the Result class with the desired data, including distance/direction calculation
                .Select(result => new ResultModel
                {
                    Name = result.Name,
                    Distance = GeoCalculator.GetDistance(originCoordinate.Latitude, originCoordinate.Longitude, result.Latitude, result.Longitude, distanceUnit: distanceUnit),
                    Direction = GeoCalculator.GetDirection(originCoordinate.Latitude, originCoordinate.Longitude, result.Latitude, result.Longitude)
                })
                // Filter by distance. This is necessary because a radius is a circle, yet we've defined a square around the origin coordinate.
                // This filter removes any extraneous results that would appear in the square's "corners" (imagine placing a circle inside a square of the
                // same size for visualization).
                .Where(x => x.Distance <= radius)
                // Sort by distance
                .OrderBy(x => x.Distance);
        }

        private IEnumerable<LocationModel> Locations
        {
            get
            {
                // Simulated database table of locations that have already been geocoded with lat/long values
                // Should Spago really be in the same list as Denny's? :)
                return new List<LocationModel>
                {
                    new LocationModel { Name = "Goat & Vine", Latitude = 34.077237, Longitude = -118.395422 },
                    new LocationModel { Name = "Spago", Latitude = 34.0675918, Longitude = -118.3977091 },
                    new LocationModel { Name = "Jack N Jills Too", Latitude = 34.0734937, Longitude = -118.3830596 },
                    new LocationModel { Name = "Sushi Dokoro Ki Ra La", Latitude = 34.0394848, Longitude = -118.4657892 },
                    new LocationModel { Name = "La Dog Friendly", Latitude = 34.048207, Longitude = -118.378545 },
                    new LocationModel { Name = "Yokohama Sushi", Latitude = 34.0175319, Longitude = -118.4060849 },
                    new LocationModel { Name = "Jaragua Restaurant", Latitude = 34.0763616, Longitude = -118.3067551 },
                    new LocationModel { Name = "Denny's", Latitude = 33.9597, Longitude = -118.3806253 },
                    new LocationModel { Name = "Tony's Burger", Latitude = 34.0224905, Longitude = -118.252509 },
                    new LocationModel { Name = "Kevaccino's", Latitude = 33.9229397, Longitude = -118.4318876 },
                    new LocationModel { Name = "El Tepeyac", Latitude = 34.0819371, Longitude = -118.1464453 },
                    new LocationModel { Name = "Panda King", Latitude = 33.9169722, Longitude = -118.0865923 },
                    new LocationModel { Name = "Getty Villa Cafe", Latitude = 34.0419057, Longitude = -118.5676673 },
                    new LocationModel { Name = "Valley Inn Restaurant", Latitude = 34.1536314, Longitude = -118.4692688 },
                    new LocationModel { Name = "Leonor's Restaurant", Latitude = 34.1868785, Longitude = -118.3792344 },
                };
            }
        }
    }
}