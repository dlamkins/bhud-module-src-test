using System;
using Microsoft.Xna.Framework;

namespace Soeed.GuildGeoGuesser.Utils
{
	public static class RainbowColors
	{
		public static Color ColorFromHue(float hue)
		{
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			hue -= (float)Math.Floor(hue);
			float num = hue * 6f;
			int i = (int)Math.Floor(num);
			float f = num - (float)i;
			float q = 1f - f;
			float r;
			float g;
			float b;
			switch (i % 6)
			{
			case 0:
				r = 1f;
				g = f;
				b = 0f;
				break;
			case 1:
				r = q;
				g = 1f;
				b = 0f;
				break;
			case 2:
				r = 0f;
				g = 1f;
				b = f;
				break;
			case 3:
				r = 0f;
				g = q;
				b = 1f;
				break;
			case 4:
				r = f;
				g = 0f;
				b = 1f;
				break;
			default:
				r = 1f;
				g = 0f;
				b = q;
				break;
			}
			return new Color(r, g, b);
		}

		public static Color ColorAtAngle(double angleRadians, double hueOffsetRadians = 0.0)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			return ColorFromHue((float)(((angleRadians + hueOffsetRadians) / (Math.PI * 2.0) + 1.0) % 1.0));
		}

		public static Color ShiftingColor(float position01, float timeSeconds, float cycleSeconds = 4f)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return ColorFromHue((position01 + timeSeconds / cycleSeconds) % 1f);
		}
	}
}
