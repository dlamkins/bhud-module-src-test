using System.Collections.Generic;
using System.Linq;
using TmfLib;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing
{
	public class SharedPackCollection : IPackCollection
	{
		public PathingCategory Categories { get; private set; }

		public IList<PointOfInterest> PointsOfInterest { get; private set; }

		internal SharedPackCollection(PathingCategory categories = null, IEnumerable<PointOfInterest> pointsOfInterest = null)
		{
			Categories = categories ?? new PathingCategory(root: true);
			PointsOfInterest = pointsOfInterest?.ToList() ?? new List<PointOfInterest>();
		}

		public void Unload()
		{
		}
	}
}
