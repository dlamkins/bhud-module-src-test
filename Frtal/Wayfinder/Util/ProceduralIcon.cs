using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frtal.Wayfinder.Util
{
	public static class ProceduralIcon
	{
		public static Texture2D CreatePositionMarker(GraphicsDevice device, int size = 48)
		{
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Expected O, but got Unknown
			Color[] data = (Color[])(object)new Color[size * size];
			float c = (float)(size - 1) / 2f;
			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					int inner = 0;
					int outer = 0;
					for (int sy = 0; sy < 3; sy++)
					{
						for (int sx = 0; sx < 3; sx++)
						{
							float x2 = ((float)x + ((float)sx + 0.5f) / 3f - 0.5f - c) / ((float)size * 0.5f);
							float py = ((float)y + ((float)sy + 0.5f) / 3f - 0.5f - c) / ((float)size * 0.5f);
							if (InArrow(x2, py, 0f))
							{
								inner++;
							}
							if (InArrow(x2, py, 0.16f))
							{
								outer++;
							}
						}
					}
					int total = 9;
					if (inner > 0)
					{
						byte a2 = (byte)(255 * inner / total);
						data[y * size + x] = new Color(a2, a2, a2, a2);
					}
					else if (outer > 0)
					{
						byte a = (byte)(220 * outer / total);
						data[y * size + x] = new Color((byte)0, (byte)0, (byte)0, a);
					}
				}
			}
			Texture2D val = new Texture2D(device, size, size, false, (SurfaceFormat)0);
			val.SetData<Color>(data);
			return val;
		}

		private static bool InArrow(float x, float y, float grow)
		{
			float tipY = -0.8f - grow;
			float baseY = 0.55f + grow;
			float notchY = 0.18f + grow;
			float halfW = 0.62f + grow;
			if (y < tipY || y > baseY)
			{
				return false;
			}
			float t = (y - tipY) / (baseY - tipY);
			float w = halfW * t;
			if (Math.Abs(x) > w)
			{
				return false;
			}
			if (y > notchY)
			{
				float nt = (y - notchY) / (baseY - notchY);
				if (Math.Abs(x) < w * (1f - nt) * 0.85f)
				{
					return false;
				}
			}
			return true;
		}

		public static Texture2D CreateCompassIcon(GraphicsDevice device, int size = 64, bool hover = false)
		{
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Expected O, but got Unknown
			Color[] data = (Color[])(object)new Color[size * size];
			float c = (float)(size - 1) / 2f;
			float rRing = (float)size * 0.455f;
			float ringHalf = (float)size * (hover ? 0.055f : 0.042f);
			float starLong = (float)size * 0.36f;
			float starWide = (float)size * 0.085f;
			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					int hits = 0;
					for (int sy = 0; sy < 3; sy++)
					{
						for (int sx = 0; sx < 3; sx++)
						{
							float px = (float)x + ((float)sx + 0.5f) / 3f - 0.5f;
							float num = (float)y + ((float)sy + 0.5f) / 3f - 0.5f;
							float dx = px - c;
							float dy = num - c;
							if (Math.Abs((float)Math.Sqrt(dx * dx + dy * dy) - rRing) <= ringHalf)
							{
								hits++;
								continue;
							}
							float num2 = Math.Abs(dx);
							float ady = Math.Abs(dy);
							bool vertical = num2 / starWide + ady / starLong <= 1f;
							bool horizontal = num2 / starLong + ady / starWide <= 1f;
							if (vertical || horizontal)
							{
								hits++;
							}
						}
					}
					byte a = (byte)(255 * hits / 9);
					if (hover && a > 0)
					{
						a = (byte)Math.Min(255, a + 40);
					}
					data[y * size + x] = new Color(a, a, a, a);
				}
			}
			Texture2D val = new Texture2D(device, size, size, false, (SurfaceFormat)0);
			val.SetData<Color>(data);
			return val;
		}
	}
}
