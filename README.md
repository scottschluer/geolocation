**USAGE:**

All methods support passing Coordinate objects as parameters as well as decimal lat/long values. Assuming the following
Coordinate objects, the examples below show two different ways to make the same call:

```c#
Coordinate origin = new Coordinate(34.0675918, -118.3977091);
Coordinate destination = new Coordinate(34.076234, -118.395314);
```

Find the distance between two points:
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

// Select from all of the locations
var results = _locations
  // Where the location's latitude is between the min and max latitude boundaries
  .Where(x => x.Latitude >= minLatitude && x.Latitude <= maxLatitude)
  // And where the location's longitude is between the min and max longitude boundaries
  .Where(x => x.Longitude >= minLongitude && x.Longitude <= maxLongitude)
  // Populate an anonymous object with the desired data, including distance/direction calculation
  .Select(result => new 
  {
    Name = result.Name,
    Distance = GeoCalculator.GetDistance(originCoordinate.Latitude, originCoordinate.Longitude, result.Latitude, result.Longitude, 1),
    Direction = GeoCalculator.GetDirection(originCoordinate.Latitude, originCoordinate.Longitude, result.Latitude, result.Longitude)
  })
  // Filter by distance. This is necessary because a radius is a circle, yet we've defined a square around the origin coordinate.
  // This filter removes any extraneous results that would appear in the square's "corners" (imagine placing a circle inside a square of the
  // same size for visualization).
  .Where(x => x.Distance <= 25)
  // Sort by distance
  .OrderBy(x => x.Distance);
```
