using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Manlaan.CommanderMarkers.Utils
{
	public static class SpriteBatchExtensions
	{
		private static Texture2D? _whitePixelTexture;

		private const float SideLength = 3f;

		public static SpriteBatchParameters Clone(this SpriteBatchParameters sbp)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			SpriteBatchParameters val = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			val.set_SortMode(sbp.get_SortMode());
			val.set_BlendState(sbp.get_BlendState());
			val.set_SamplerState(sbp.get_SamplerState());
			val.set_DepthStencilState(sbp.get_DepthStencilState());
			val.set_RasterizerState(sbp.get_RasterizerState());
			val.set_Effect(sbp.get_Effect());
			val.set_TransformMatrix(sbp.get_TransformMatrix());
			return val;
		}

		private static Texture2D GetTexture(SpriteBatch spriteBatch)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatch spriteBatch2 = spriteBatch;
			if (_whitePixelTexture == null)
			{
				_whitePixelTexture = new Texture2D(((GraphicsResource)spriteBatch2).get_GraphicsDevice(), 1, 1, false, (SurfaceFormat)0);
				_whitePixelTexture!.SetData<Color>((Color[])(object)new Color[1] { Color.get_White() });
				((GraphicsResource)spriteBatch2).add_Disposing((EventHandler<EventArgs>)dispose);
			}
			return _whitePixelTexture;
			void dispose(object sender, EventArgs args)
			{
				((GraphicsResource)spriteBatch2).remove_Disposing((EventHandler<EventArgs>)dispose);
				Texture2D? whitePixelTexture = _whitePixelTexture;
				if (whitePixelTexture != null)
				{
					((GraphicsResource)whitePixelTexture).Dispose();
				}
				_whitePixelTexture = null;
			}
		}

		public static void DrawPoint(this SpriteBatch spriteBatch, Vector2 position, Color color, float size = 1f, float layerDepth = 0f)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			Vector2 scale = Vector2.get_One() * size;
			Vector2 offset = new Vector2(0.5f) - new Vector2(size * 0.5f);
			spriteBatch.Draw(GetTexture(spriteBatch), position + offset, (Rectangle?)null, color, 0f, Vector2.get_Zero(), scale, (SpriteEffects)0, layerDepth);
		}

		public static void DrawPoint(this SpriteBatch spriteBatch, float x, float y, Color color, float size = 1f, float layerDepth = 0f)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawPoint(new Vector2(x, y), color, size, layerDepth);
		}

		public static void DrawPolygon(this SpriteBatch spriteBatch, Vector2 offset, IEnumerable<Vector2> points, Color color, float thickness = 1f, float layerDepth = 0f, bool open = false)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			if (!points.Any())
			{
				return;
			}
			if (!points.Skip(1).Any())
			{
				spriteBatch.DrawPoint(points.First(), color, thickness);
				return;
			}
			Texture2D texture = GetTexture(spriteBatch);
			foreach (var (point, next) in points.By2())
			{
				DrawPolygonEdge(spriteBatch, texture, point + offset, next + offset, color, thickness, layerDepth);
			}
			if (!open)
			{
				DrawPolygonEdge(spriteBatch, texture, points.Last() + offset, points.First() + offset, color, thickness, layerDepth);
			}
		}

		private static void DrawPolygonEdge(SpriteBatch spriteBatch, Texture2D texture, Vector2 point1, Vector2 point2, Color color, float thickness, float layerDepth)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			float length = Vector2.Distance(point1, point2);
			float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
			Vector2 scale = default(Vector2);
			((Vector2)(ref scale))._002Ector(length, thickness);
			spriteBatch.Draw(texture, point1, (Rectangle?)null, color, angle, Vector2.get_Zero(), scale, (SpriteEffects)0, layerDepth);
		}

		public static void DrawRectangleFill(this SpriteBatch spriteBatch, RectangleF rectangle, Color color, float layerDepth = 0f)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			float center = ((RectangleF)(ref rectangle)).get_Left() + rectangle.Width / 2f;
			ShapeExtensions.DrawLine(spriteBatch, center, ((RectangleF)(ref rectangle)).get_Top(), center, ((RectangleF)(ref rectangle)).get_Bottom(), color, rectangle.Width, layerDepth);
		}

		public static int GetSidesFor(Vector2 radii, double amplitude)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return GetSidesFor(Math.Max(radii.X, radii.Y), amplitude);
		}

		public static int GetSidesFor(float radius, double amplitude)
		{
			return Math.Max(3, (int)Math.Ceiling(Math.Abs(amplitude * Math.PI / Math.Acos(1f - MathUtils.Squared(3f) / MathUtils.Squared(radius) / 2f))));
		}

		public static void DrawArc(this SpriteBatch spriteBatch, Vector2 center, Vector2 radii, double start, double extents, int sides, Color color, float thickness = 1f, float layerDepth = 0f)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawArc(center, radii.X, radii.Y, start, extents, sides, color, thickness, layerDepth);
		}

		public static void DrawArc(this SpriteBatch spriteBatch, Vector2 center, float rx, float ry, double start, double extents, int sides, Color color, float thickness = 1f, float layerDepth = 0f)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			extents = (double.IsNaN(extents) ? 0.0 : (double.IsInfinity(extents) ? 2.0 : extents));
			rx += thickness / 2f * (float)Math.Sign(extents);
			ry += thickness / 2f * (float)Math.Sign(extents);
			spriteBatch.DrawPolygon(center, CreateArc(rx, ry, start, extents, sides), color, thickness, layerDepth, open: true);
		}

		public static Vector2[] CreateArc(float rx, float ry, double start, double extents, int sides)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			Vector2[] points = (Vector2[])(object)new Vector2[sides];
			double theta = start * Math.PI;
			double step = extents * Math.PI / (double)(sides - 1);
			Vector2 vector = default(Vector2);
			for (int i = 0; i < sides; i++)
			{
				((Vector2)(ref vector))._002Ector(0f - (float)((double)rx * Math.Cos(theta)), 0f - (float)((double)ry * Math.Sin(theta)));
				points[i] = vector;
				theta += step;
			}
			return points;
		}

		public static Vector2[] CreateEllipsis(float rx, float ry, int sides)
		{
			return CreateArc(rx, ry, 0.0, 2.0, sides);
		}

		public static Vector2[] CreateCircle(float radius, int sides)
		{
			return CreateEllipsis(radius, radius, sides);
		}
	}
}
