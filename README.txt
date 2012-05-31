USAGE:

- Find the distance between two points:

  GetDistance(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude, int decimalPlaces)

  ex: double distance = GeoCalculator.GetDistance(34.0675918, -118.3977091, 34.076234, -118.395314, 1);

- Find the cardinal direction (e.g. N, NW, W) from an origin coordinate to a destination coordinate:

  GetDirection(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude)

  ex: string direction = GeoCalculator.GetDirection(34.0675918, -118.3977091, 34.076234, -118.395314);

- Find the bearing (degrees) from an origin coordinate to a destination coordinate:

  GetBearing(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude)

  ex: double bearing = GeoCalculator.GetBearing(34.0675918, -118.3977091, 34.076234, -118.395314);

- Find the lat/long boundaries for a given origin coordinate and radius:

  CoordinateBoundaries(double latitude, double longitude, double distance)

  CoordinateBoundaries boundaries = new CoordinateBoundaries(34.0675918, -118.3977091, 25);
  double minLatitude = boundaries.MinLatitude;
  double maxLatitude = boundaries.MaxLatitude;
  double minLongitude = boundaries.MinLongitude;
  double maxLongitude = boundaries.MaxLongitude;