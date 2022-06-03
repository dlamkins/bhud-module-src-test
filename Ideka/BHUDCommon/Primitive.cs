using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Ideka.BHUDCommon
{
	public class Primitive
	{
		public List<Vector3> Points { get; }

		public Primitive(IEnumerable<Vector3> points)
		{
			Points = points.ToList();
		}

		public Primitive(params Vector3[] points)
			: this(points.AsEnumerable())
		{
		}

		public IEnumerable<Vector3> Transform(Matrix matrix)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			foreach (Vector3 p in Points)
			{
				yield return Vector3.Transform(p, matrix);
			}
		}

		public Primitive Transformed(Matrix matrix)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return new Primitive(Transform(matrix));
		}

		public IEnumerable<Vector2> ToScreen()
		{
			foreach (Vector3 p in Points)
			{
				yield return ToScreen(p);
			}
		}

		public static Primitive operator +(Primitive a, Primitive b)
		{
			return new Primitive(a.Points.Concat(b.Points));
		}

		public static Vector2 Flatten(Vector3 v)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			if (!(v.Z <= 0f))
			{
				return new Vector2((v.X / v.Z + 1f) / 2f * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Width(), (1f - v.Y / v.Z) / 2f * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
			}
			return new Vector2(float.NaN, float.NaN);
		}

		public static Vector2 ToScreen(Matrix world)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			return Flatten(Vector3.Transform(Vector3.get_Zero(), world * GameService.Gw2Mumble.get_PlayerCamera().get_WorldViewProjection()));
		}

		public static Vector2 ToScreen(Vector3 point)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ToScreen(Matrix.CreateTranslation(point));
		}

		public static Primitive HorizontalCircle(float radius, int sides)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			Vector3[] points = (Vector3[])(object)new Vector3[sides];
			double step = Math.PI * 2.0 / (double)sides;
			double theta = 0.0;
			for (int i = 0; i < sides; i++)
			{
				points[i] = new Vector3((float)((double)radius * Math.Cos(theta)), (float)((double)radius * Math.Sin(theta)), 0f);
				theta += step;
			}
			return new Primitive(points);
		}
	}
}
