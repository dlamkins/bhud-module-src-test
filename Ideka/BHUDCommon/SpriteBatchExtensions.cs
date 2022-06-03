using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.BHUDCommon
{
	internal static class SpriteBatchExtensions
	{
		private static Texture2D _whitePixelTexture;

		private static Texture2D GetTexture(SpriteBatch spriteBatch)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			if (_whitePixelTexture == null)
			{
				_whitePixelTexture = new Texture2D(((GraphicsResource)spriteBatch).get_GraphicsDevice(), 1, 1, false, (SurfaceFormat)0);
				_whitePixelTexture.SetData<Color>((Color[])(object)new Color[1] { Color.get_White() });
				((GraphicsResource)spriteBatch).add_Disposing((EventHandler<EventArgs>)delegate
				{
					Texture2D whitePixelTexture = _whitePixelTexture;
					if (whitePixelTexture != null)
					{
						((GraphicsResource)whitePixelTexture).Dispose();
					}
					_whitePixelTexture = null;
				});
			}
			return _whitePixelTexture;
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
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
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
			foreach (var (point, next) in points.Zip(points.Skip(1), (Vector2 a, Vector2 b) => (a, b)))
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
	}
}
