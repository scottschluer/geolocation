[![][nuget-img]][nuget]

[nuget]:     https://www.nuget.org/packages/Geolocation
[nuget-img]: https://badge.fury.io/nu/geolocation.svg

**USAGE:**

All methods support passing Coordinate objects as parameters as well as decimal lat/long values. Assuming the following
Coordinate objects, the examples below show two different ways to make the same call:

```c#
Coordinate origin = new Coordinate(34.0675918, -118.3977091);
Coordinate destination = new Coordinate(34.076234, -118.395314);
```

Find the distance between two points in miles:
```c#
double distance = GeoCalculator.GetDistance(34.0675918, -118.3977091, 34.076234, -118.395314, 1);
//OR
double distance = GeoCalculator.GetDistance(origin, destination, 1);
```
Find the cardinal direction (e.g. N, NW, W) from an origin coordinate to a destination coordinate:
```c#
string direction = GeoCalculator.GetDirection(34.0675918, -118.3977091, 34.076234, -118.395314);
//OR
string direction = GeoCalculator.GetDirection(origin, destination);
```

Find the bearing (degrees) from an origin coordinate to a destination coordinate:
```c#
double bearing = GeoCalculator.GetBearing(34.0675918, -118.3977091, 34.076234, -118.395314);
//OR
double bearing = GeoCalculator.GetBearing(origin, destination);
```

Find the lat/long boundaries for a given origin coordinate and radius:
```c#
CoordinateBoundaries boundaries = new CoordinateBoundaries(34.0675918, -118.3977091, 25);
//OR
CoordinateBoundaries boundaries = new CoordinateBoundaries(origin, 25);
  
double minLatitude = boundaries.MinLatitude;
double maxLatitude = boundaries.MaxLatitude;
double minLongitude = boundaries.MinLongitude;
double maxLongitude = boundaries.MaxLongitude;

var results = _locations
  .Where(x => x.Latitude >= minLatitude && x.Latitude <= maxLatitude)
  .Where(x => x.Longitude >= minLongitude && x.Longitude <= maxLongitude)
  .Select(result => new 
  {
    Name = result.Name,
    Distance = GeoCalculator.GetDistance(origin.Latitude, origin.Longitude, result.Latitude, result.Longitude, 1),
    Direction = GeoCalculator.GetDirection(origin.Latitude, origin.Longitude, result.Latitude, result.Longitude)
  })
  .Where(x => x.Distance <= 25)
  .OrderBy(x => x.Distance);
```
