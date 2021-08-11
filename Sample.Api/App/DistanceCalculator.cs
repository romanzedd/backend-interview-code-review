using Sample.Api.Model;
using System;

namespace Sample.Api.App
{
	public sealed class DistanceCalculator
	{
		const double PIx = 3.141592653589793; // Since we are already using Math lib, why not use Math.PI instead.
		const double RADIUS = 6378.16; // It's better to be consistent with naming, in this case I suggest rely on C# convention - Radius, or rather EquatorialRadius or EarthRadius just to make it clear.

		private double Radians(double x)
		{
			return x * PIx / 180;
		}

		public double DistanceBetweenPlaces(double lon1, double lat1, double lon2, double lat2)
		{
			double dlon = Radians(lon2 - lon1);
			double dlat = Radians(lat2 - lat1);

			// It's not quite clear what intermediate values are, a comment that it is a haversine calculation would be much appreciated here.
			// We don't really need imlicit declaration here, use 'var a' and 'var angle' instead.
			double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
			// Using Math.Pow() instead of multiplying values would get rid of unnecessary parenthesis and help make expression easier to comprehend.
			// var a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * Math.Pow(Math.Sin(dlon / 2), 2);
			double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			return angle * RADIUS;
		}

		// Do we really need this overload? Why not pass Airport objs to the first method, since we use it's properties only once there anyway?
		public double DistanceBetweenPlaces(Airport first, Airport second)
		{
			return DistanceBetweenPlaces(first.Longitude, first.Latitude, second.Longitude, second.Latitude);
		}
	}
}