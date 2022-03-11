using System;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.Helpers
{
	internal static class ColorHelper
	{
		private struct HSLColor
		{
			public float H;

			public float S;

			public float L;

			public HSLColor(float h, float s, float l)
			{
				H = h;
				S = s;
				L = l;
			}

			public static HSLColor FromColor(Color color)
			{
				return FromRgb(((Color)(ref color)).get_R(), ((Color)(ref color)).get_G(), ((Color)(ref color)).get_B());
			}

			public static HSLColor FromRgb(byte R, byte G, byte B)
			{
				HSLColor hSLColor = default(HSLColor);
				hSLColor.H = 0f;
				hSLColor.S = 0f;
				hSLColor.L = 0f;
				HSLColor hsl = hSLColor;
				float r = (float)(int)R / 255f;
				float g = (float)(int)G / 255f;
				float b = (float)(int)B / 255f;
				float min = Math.Min(Math.Min(r, g), b);
				float max = Math.Max(Math.Max(r, g), b);
				float delta = max - min;
				hsl.L = (max + min) / 2f;
				if (delta > 0f)
				{
					if (hsl.L < 0.5f)
					{
						hsl.S = delta / (max + min);
					}
					else
					{
						hsl.S = delta / (2f - max - min);
					}
					float deltaR = ((max - r) / 6f + delta / 2f) / delta;
					float deltaG = ((max - g) / 6f + delta / 2f) / delta;
					float deltaB = ((max - b) / 6f + delta / 2f) / delta;
					if (r == max)
					{
						hsl.H = deltaB - deltaG;
					}
					else if (g == max)
					{
						hsl.H = 0.33333334f + deltaR - deltaB;
					}
					else if (b == max)
					{
						hsl.H = 2f / 3f + deltaG - deltaR;
					}
					if (hsl.H < 0f)
					{
						hsl.H += 1f;
					}
					if (hsl.H > 1f)
					{
						hsl.H -= 1f;
					}
				}
				return hsl;
			}

			public HSLColor GetComplement()
			{
				float h = H + 0.5f;
				if (h > 1f)
				{
					h -= 1f;
				}
				return new HSLColor(h, S, L);
			}

			public Color ToRgbColor()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
				Color c = default(Color);
				if (S == 0f)
				{
					((Color)(ref c)).set_R((byte)(L * 255f));
					((Color)(ref c)).set_G((byte)(L * 255f));
					((Color)(ref c)).set_B((byte)(L * 255f));
				}
				else
				{
					float v2 = L + S - S * L;
					if (L < 0.5f)
					{
						v2 = L * (1f + S);
					}
					float v1 = 2f * L - v2;
					((Color)(ref c)).set_R((byte)(255f * HueToRgb(v1, v2, H + 0.33333334f)));
					((Color)(ref c)).set_G((byte)(255f * HueToRgb(v1, v2, H)));
					((Color)(ref c)).set_B((byte)(255f * HueToRgb(v1, v2, H - 0.33333334f)));
				}
				return c;
			}

			private static float HueToRgb(float v1, float v2, float vH)
			{
				vH += (float)((vH < 0f) ? 1 : 0);
				vH -= (float)((vH > 1f) ? 1 : 0);
				float ret = v1;
				if (6f * vH < 1f)
				{
					ret = v1 + (v2 - v1) * 6f * vH;
				}
				else if (2f * vH < 1f)
				{
					ret = v2;
				}
				else if (3f * vH < 2f)
				{
					ret = v1 + (v2 - v1) * (2f / 3f - vH) * 6f;
				}
				return Math.Max(Math.Min(ret, 1f), 0f);
			}
		}

		public static Color GetComplementary(this Color color)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			HSLColor hsl = HSLColor.FromColor(color);
			hsl.H += 0.5f;
			if (hsl.H > 1f)
			{
				hsl.H -= 1f;
			}
			Color switchedColor = hsl.ToRgbColor();
			((Color)(ref switchedColor)).set_A(((Color)(ref color)).get_A());
			return switchedColor;
		}

		private static float HueToRGB(float one, float two, float hue)
		{
			if (hue < 0f)
			{
				hue += 1f;
			}
			if (hue > 1f)
			{
				hue -= 1f;
			}
			if (6f * hue < 1f)
			{
				return one + (two - one) * 6f * hue;
			}
			if (2f * hue < 1f)
			{
				return two;
			}
			if (3f * hue < 2f)
			{
				return one + (two - one) * (0f - hue) * 6f;
			}
			return one;
		}
	}
}
