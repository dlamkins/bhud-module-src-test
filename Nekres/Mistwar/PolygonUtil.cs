using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Nekres.Mistwar
{
	public static class PolygonUtil
	{
		private static double Cross(Vector2 O, Vector2 A, Vector2 B)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			return (A.X - O.X) * (B.Y - O.Y) - (A.Y - O.Y) * (B.X - O.X);
		}

		public static List<Vector2> Get(List<Vector2> points)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
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
			List<Vector2> H = new List<Vector2>((IEnumerable<Vector2>)(object)new Vector2[2 * l]);
			points.Sort((Vector2 a, Vector2 b) => (a.X != b.X) ? a.X.CompareTo(b.X) : a.Y.CompareTo(b.Y));
			for (int j = 0; j < l; j++)
			{
				while (k >= 2 && Cross(H[k - 2], H[k - 1], points[j]) <= 0.0)
				{
					k--;
				}
				H[k++] = points[j];
			}
			int i = l - 2;
			int t = k + 1;
			while (i >= 0)
			{
				while (k >= t && Cross(H[k - 2], H[k - 1], points[i]) <= 0.0)
				{
					k--;
				}
				H[k++] = points[i];
				i--;
			}
			return H.Take(k - 1).ToList();
		}

		public static bool InBounds(Vector2 point, IReadOnlyList<Vector2> hull)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
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

		public static bool IsPointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			Vector2 a2 = b - a;
			Vector2 bc = c - b;
			Vector2 ca = a - c;
			Vector2 ap = p - a;
			Vector2 bp = p - b;
			Vector2 cp = p - c;
			float num = MathUtil.Cross(a2, ap);
			float c2 = MathUtil.Cross(bc, bp);
			float c3 = MathUtil.Cross(ca, cp);
			if (num < 0f && c2 < 0f && c3 < 0f)
			{
				return true;
			}
			return false;
		}

		public static bool Triangulate(Vector2[] vertices, out int[] triangles)
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			triangles = null;
			if (vertices == null || vertices.Length < 3 || vertices.Length > 1024)
			{
				return false;
			}
			List<int> indexList = new List<int>();
			for (int j = 0; j < vertices.Length; j++)
			{
				indexList.Add(j);
			}
			int totalTriangleIndexCount = (vertices.Length - 2) * 3;
			triangles = new int[totalTriangleIndexCount];
			int triangleIndexCount = 0;
			while (indexList.Count > 3)
			{
				for (int i = 0; i < indexList.Count; i++)
				{
					int a = indexList[i];
					int b = indexList.GetItem(i - 1);
					int c = indexList.GetItem(i + 1);
					Vector2 va = vertices[a];
					Vector2 vb = vertices[b];
					Vector2 vc = vertices[c];
					Vector2 a2 = vb - va;
					Vector2 va_to_vc = vc - va;
					if (MathUtil.Cross(a2, va_to_vc) < 0f)
					{
						continue;
					}
					bool isEar = true;
					for (int k = 0; k < vertices.Length; k++)
					{
						if (k != a && k != b && k != c && IsPointInTriangle(vertices[k], vb, va, vc))
						{
							isEar = false;
							break;
						}
					}
					if (isEar)
					{
						triangles[triangleIndexCount++] = b;
						triangles[triangleIndexCount++] = a;
						triangles[triangleIndexCount++] = c;
						indexList.RemoveAt(i);
						break;
					}
				}
			}
			triangles[triangleIndexCount++] = indexList[0];
			triangles[triangleIndexCount++] = indexList[1];
			triangles[triangleIndexCount++] = indexList[2];
			return true;
		}
	}
}
