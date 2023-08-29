using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Presets
{
	public class Primitive
	{
		public class ScreenPrimitive
		{
			public List<List<Vector2>> Points { get; } = new List<List<Vector2>>();


			public float Depth { get; }

			private float MaxDepth => GameService.Gw2Mumble.get_PlayerCamera().get_FarPlaneRenderDistance();

			public ScreenPrimitive(IEnumerable<Vector3> points)
			{
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				float sum = 0f;
				int count = 0;
				List<Vector2> list = null;
				foreach (Vector3 point in points)
				{
					if (list == null)
					{
						list = new List<Vector2>();
						Points.Add(list);
					}
					if (point.Z < 0f)
					{
						list = null;
						continue;
					}
					list.Add(Flatten(point));
					sum += point.Z;
					count++;
				}
				Depth = sum / (float)count / MaxDepth;
			}

			public void Draw(SpriteBatch spriteBatch, Color color, float thickness)
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				foreach (List<Vector2> list in Points)
				{
					spriteBatch.DrawPolygon(Vector2.get_Zero(), list, color, thickness, Depth, open: true);
				}
			}

			public static Vector2 Flatten(Vector3 v)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				return new Vector2((v.X / v.Z + 1f) / 2f * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Width(), (1f - v.Y / v.Z) / 2f * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
			}
		}

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

		public IEnumerable<Vector2> Flat()
		{
			return Points.Select((Vector3 p) => p.ToVector2());
		}

		public ScreenPrimitive ToScreen()
		{
			return new ScreenPrimitive(Points.Select(ToScreen));
		}

		public static Primitive operator +(Primitive a, Primitive b)
		{
			return new Primitive(a.Points.Concat(b.Points));
		}

		public static Vector3 ToScreen(Vector3 point)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return Vector3.Transform(point, GameService.Gw2Mumble.get_PlayerCamera().get_WorldViewProjection());
		}

		public static Primitive HorizontalCircle(float radius, int sides)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			Vector2[] points2d = SpriteBatchExtensions.CreateCircle(radius, sides);
			Vector3[] points = (Vector3[])(object)new Vector3[sides];
			for (int i = 0; i < sides; i++)
			{
				Vector2 src = points2d[i];
				points[i] = new Vector3(src.X, src.Y, 0f);
			}
			return new Primitive(points);
		}

		public static Primitive VerticalArc(float rx, float ry, float start, float extents, int sides)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			Vector2[] points2d = SpriteBatchExtensions.CreateArc(rx, ry, start, extents, sides);
			Vector3[] points = (Vector3[])(object)new Vector3[sides];
			for (int i = 0; i < sides; i++)
			{
				Vector2 src = points2d[i];
				points[i] = new Vector3(src.X, 0f, src.Y);
			}
			return new Primitive(points);
		}
	}
}
