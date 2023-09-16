using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;

namespace Nekres.Regions_Of_Tyria.Geometry
{
	public class Sector
	{
		private struct Point
		{
			public readonly double X;

			public readonly double Y;

			public Point(double x, double y)
			{
				X = x;
				Y = y;
			}
		}

		public readonly int Id;

		public readonly string Name;

		private readonly IReadOnlyList<Point> _bounds;

		public Sector(ContinentFloorRegionMapSector sector)
		{
			Id = sector.get_Id();
			Name = sector.get_Name();
			_bounds = (from b in sector.get_Bounds()
				select new Point(((Coordinates2)(ref b)).get_X(), ((Coordinates2)(ref b)).get_Y())).ToList();
		}

		public bool Contains(double x, double y)
		{
			return Contains(new Point(x, y), _bounds);
		}

		private bool Contains(Point targetPoint, IReadOnlyList<Point> polygon)
		{
			if (polygon.Count < 3)
			{
				return false;
			}
			double x = targetPoint.X;
			double y = targetPoint.Y;
			bool isInside = false;
			int i = 0;
			int j = polygon.Count - 1;
			while (i < polygon.Count)
			{
				if (polygon[i].Y > y != polygon[j].Y > y && x < (polygon[j].X - polygon[i].X) * (y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
				{
					isInside = !isInside;
				}
				j = i++;
			}
			return isInside;
		}
	}
}
