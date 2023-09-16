using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Nekres.Regions_Of_Tyria.Geometry
{
	public class Sector
	{
		public readonly int Id;

		public readonly string Name;

		public readonly List<Point> Bounds;

		public Sector(ContinentFloorRegionMapSector sector)
		{
			Id = sector.get_Id();
			Name = sector.get_Name();
			Bounds = (from b in sector.get_Bounds()
				select b.ToPoint()).ToList();
		}
	}
}
