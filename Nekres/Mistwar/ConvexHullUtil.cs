using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Nekres.Mistwar
{
	public static class ConvexHullUtil
	{
		private static double Cross(PointF O, PointF A, PointF B)
		{
			return (A.X - O.X) * (B.Y - O.Y) - (A.Y - O.Y) * (B.X - O.X);
		}

		public static List<PointF> Get(List<PointF> points)
		{
			if (points == null)
			{
				return null;
			}
			if (points.Count() <= 1)
			{
				return points;
			}
			int l = points.Count();
			int k = 0;
			List<PointF> H = new List<PointF>(new PointF[2 * l]);
			points.Sort((PointF a, PointF b) => (a.X == b.X) ? a.Y.CompareTo(b.Y) : a.X.CompareTo(b.X));
			for (int i = 0; i < l; i++)
			{
				while (k >= 2 && Cross(H[k - 2], H[k - 1], points[i]) <= 0.0)
				{
					k--;
				}
				H[k++] = points[i];
			}
			int j = l - 2;
			int t = k + 1;
			while (j >= 0)
			{
				while (k >= t && Cross(H[k - 2], H[k - 1], points[j]) <= 0.0)
				{
					k--;
				}
				H[k++] = points[j];
				j--;
			}
			return H.Take(k - 1).ToList();
		}

		public static bool InBounds(Point point, IReadOnlyList<Point> hull)
		{
			bool result = false;
			int j = hull.Count() - 1;
			for (int i = 0; i < hull.Count(); i++)
			{
				if (((hull[i].Y < point.Y && hull[j].Y >= point.Y) || (hull[j].Y < point.Y && hull[i].Y >= point.Y)) && hull[i].X + (point.Y - hull[i].Y) / (hull[j].Y - hull[i].Y) * (hull[j].X - hull[i].X) < point.X)
				{
					result = !result;
				}
				j = i;
			}
			return result;
		}
	}
}
