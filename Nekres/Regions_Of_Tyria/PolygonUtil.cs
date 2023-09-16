using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Nekres.Regions_Of_Tyria
{
	public static class PolygonUtil
	{
		public static bool IsPointInsidePolygon(Point targetPoint, List<Point> polygon)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
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
				if ((double)polygon[i].Y > y != (double)polygon[j].Y > y && x < (double)(polygon[j].X - polygon[i].X) * (y - (double)polygon[i].Y) / (double)(polygon[j].Y - polygon[i].Y) + (double)polygon[i].X)
				{
					isInside = !isInside;
				}
				j = i++;
			}
			return isInside;
		}
	}
}
