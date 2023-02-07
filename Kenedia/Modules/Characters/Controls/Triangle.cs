using System;
using System.Collections.Generic;
using System.Drawing;
using Kenedia.Modules.Core.Extensions;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Controls
{
	public struct Triangle
	{
		public static Triangle Empty = new Triangle(new Vector2(0f), new Vector2(0f), new Vector2(0f));

		public Vector2 Point1 { get; set; }

		public Vector2 Point2 { get; set; }

		public Vector2 Point3 { get; set; }

		public Triangle(Vector2 point1, Vector2 point2, Vector2 point3)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Point1 = point1;
			Point2 = point2;
			Point3 = point3;
		}

		public bool CompareTo(Triangle t)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = Point1;
			if (((Vector2)(ref val)).Equals(t.Point1))
			{
				val = Point2;
				if (((Vector2)(ref val)).Equals(t.Point2))
				{
					val = Point3;
					return ((Vector2)(ref val)).Equals(t.Point3);
				}
			}
			return false;
		}

		public bool IsEmpty()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = Point1;
			if (((Vector2)(ref val)).Equals(Empty.Point1))
			{
				val = Point2;
				if (((Vector2)(ref val)).Equals(Empty.Point2))
				{
					val = Point3;
					return ((Vector2)(ref val)).Equals(Empty.Point3);
				}
			}
			return false;
		}

		public List<Vector2> ToVectorList()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			return new List<Vector2> { Point1, Point2, Point3 };
		}

		public bool Contains(Vector2 pt)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			float num = Sign(pt, Point1, Point2);
			float d2 = Sign(pt, Point2, Point3);
			float d3 = Sign(pt, Point3, Point1);
			bool has_neg = num < 0f || d2 < 0f || d3 < 0f;
			bool has_pos = num > 0f || d2 > 0f || d3 > 0f;
			return !(has_neg && has_pos);
		}

		private float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
		}

		public Point LowestRectPoint()
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			Vector2 min = default(Vector2);
			((Vector2)(ref min))._002Ector(Math.Min(Point1.X, Math.Min(Point2.X, Point3.X)), Math.Min(Point1.Y, Math.Min(Point2.Y, Point3.Y)));
			return new Point((int)min.X, (int)min.Y);
		}

		public List<PointF> DrawingPoints()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			float diff_X = Point2.X - Point1.X;
			float num = Point2.Y - Point1.Y;
			Vector2 val = Point2;
			Point p = ((Vector2)(ref val)).ToPoint();
			val = Point1;
			int pointNum = p.Distance2D(((Vector2)(ref val)).ToPoint());
			float interval_X = diff_X / (float)(pointNum + 1);
			float interval_Y = num / (float)(pointNum + 1);
			List<PointF> pointList = new List<PointF>();
			for (int i = 1; i <= pointNum; i++)
			{
				pointList.Add(new PointF(Point1.X + interval_X * (float)i, Point1.Y + interval_Y * (float)i));
			}
			return pointList;
		}
	}
}
