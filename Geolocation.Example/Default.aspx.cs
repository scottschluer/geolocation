using System;
using System.Linq;
using System.Collections.Generic;
using Geolocation;

namespace Geolocation.Example
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly List<Location> _locations = new List<Location>();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Simulated database table of locations that have already been geocoded with lat/long values
            // Should Spago really be in the same list as Denny's? :)
            _locations.Add(new Location { Name = "Spago", Latitude = 34.0675918, Longitude = -118.3977091 });
            _locations.Add(new Location { Name = "Jack N Jills Too", Latitude = 34.0734937, Longitude = -118.3830596 });
            _locations.Add(new Location { Name = "Sushi Dokoro Ki Ra La", Latitude = 34.0394848, Longitude = -118.4657892 });
            _locations.Add(new Location { Name = "La Dog Friendly", Latitude = 34.048207, Longitude = -118.378545 });
            _locations.Add(new Location { Name = "Yokohama Sushi", Latitude = 34.0175319, Longitude = -118.4060849 });
            _locations.Add(new Location { Name = "Jaragua Restaurant", Latitude = 34.0763616, Longitude = -118.3067551 });
            _locations.Add(new Location { Name = "Denny's", Latitude = 33.9597, Longitude = -118.3806253 });
            _locations.Add(new Location { Name = "Tony's Burger", Latitude = 34.0224905, Longitude = -118.252509 });
            _locations.Add(new Location { Name = "Kevaccino's", Latitude = 33.9229397, Longitude = -118.4318876 });
            _locations.Add(new Location { Name = "El Tepeyac", Latitude = 34.0819371, Longitude = -118.1464453 });
            _locations.Add(new Location { Name = "Panda King", Latitude = 33.9169722, Longitude = -118.0865923 });
            _locations.Add(new Location { Name = "Getty Villa Cafe", Latitude = 34.0419057, Longitude = -118.5676673 });
            _locations.Add(new Location { Name = "Valley Inn Restaurant", Latitude = 34.1536314, Longitude = -118.4692688 });
            _locations.Add(new Location { Name = "Leonor's Restaurant", Latitude = 34.1868785, Longitude = -118.3792344 });
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            double radius = Convert.ToDouble(Radius.SelectedValue);

            // These coordinates would normally be retrieved from a geocoding API such as Google's or Bing's.
            // See https://github.com/scottschluer/Geocoder for an easy to use Geocoder API for Google.
            Coordinate originCoordinate = new Coordinate { Latitude = 34.076234, Longitude = -118.395314 };

            // Get the boundaries (min and max) latitude and longitude values. This forms a "square" around the origin coordinate
            // with each leg of the square exactly "X" miles from the origin, where X is the selected radius.
            CoordinateBoundaries boundaries = new CoordinateBoundaries(originCoordinate.Latitude, originCoordinate.Longitude, radius);

            // Select from all of the locations
            IEnumerable<Result> results = _locations
                // Where the location's latitude is between the min and max latitude boundaries
                .Where(x => x.Latitude >= boundaries.MinLatitude && x.Latitude <= boundaries.MaxLatitude)
                // And where the location's longitude is between the min and max longitude boundaries
                .Where(x => x.Longitude >= boundaries.MinLongitude && x.Longitude <= boundaries.MaxLongitude)
                // Populate an instance of the Result class with the desired data, including distance/direction calculation
                .Select(result => new Result
                {
                    Name = result.Name,
                    Distance = GeoCalculator.GetDistance(originCoordinate.Latitude, originCoordinate.Longitude, result.Latitude, result.Longitude, 1),
                    Direction = GeoCalculator.GetDirection(originCoordinate.Latitude, originCoordinate.Longitude, result.Latitude, result.Longitude)
                })
                // Filter by distance. This is necessary because a radius is a circle, yet we've defined a square around the origin coordinate.
                // This filter removes any extraneous results that would appear in the square's "corners" (imagine placing a circle inside a square of the
                // same size for visualization).
                .Where(x => x.Distance <= radius)
                // Sort by distance
                .OrderBy(x => x.Distance);

            gvLocations.DataSource = results;
            gvLocations.DataBind();
        }
    }
}