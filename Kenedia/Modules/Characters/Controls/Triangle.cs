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
			Point1 = point1;
			Point2 = point2;
			Point3 = point3;
		}

		public bool CompareTo(Triangle t)
		{
			if (Point1.Equals(t.Point1) && Point2.Equals(t.Point2))
			{
				return Point3.Equals(t.Point3);
			}
			return false;
		}

		public bool IsEmpty()
		{
			if (Point1.Equals(Empty.Point1) && Point2.Equals(Empty.Point2))
			{
				return Point3.Equals(Empty.Point3);
			}
			return false;
		}

		public List<Vector2> ToVectorList()
		{
			return new List<Vector2>(3) { Point1, Point2, Point3 };
		}

		public bool PointInTriangle(Vector2 p)
		{
			Vector2 vector = Point3 - Point1;
			Vector2 v2 = Point2 - Point1;
			Vector2 v3 = p - Point1;
			float dot0 = Vector2.Dot(vector, vector);
			float dot = Vector2.Dot(vector, v2);
			float dot2 = Vector2.Dot(vector, v3);
			float dot3 = Vector2.Dot(v2, v2);
			float dot4 = Vector2.Dot(v2, v3);
			float denom = dot0 * dot3 - dot * dot;
			if (Math.Abs(denom) < float.Epsilon)
			{
				return false;
			}
			float invDenom = 1f / denom;
			float u = (dot3 * dot2 - dot * dot4) * invDenom;
			float v = (dot0 * dot4 - dot * dot2) * invDenom;
			if (u >= 0f && v >= 0f)
			{
				return u + v <= 1f;
			}
			return false;
		}

		public bool Contains(Vector2 pt)
		{
			float num = Sign(pt, Point1, Point2);
			float d2 = Sign(pt, Point2, Point3);
			float d3 = Sign(pt, Point3, Point1);
			bool has_neg = num < 0f || d2 < 0f || d3 < 0f;
			bool has_pos = num > 0f || d2 > 0f || d3 > 0f;
			return !(has_neg && has_pos);
		}

		private float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
		{
			return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
		}

		public Microsoft.Xna.Framework.Point LowestRectPoint()
		{
			Vector2 min = new Vector2(Math.Min(Point1.X, Math.Min(Point2.X, Point3.X)), Math.Min(Point1.Y, Math.Min(Point2.Y, Point3.Y)));
			return new Microsoft.Xna.Framework.Point((int)min.X, (int)min.Y);
		}

		public List<PointF> DrawingPoints()
		{
			float diff_X = Point2.X - Point1.X;
			float num = Point2.Y - Point1.Y;
			int pointNum = Point2.ToPoint().Distance2D(Point1.ToPoint());
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
