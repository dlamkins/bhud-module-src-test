using System;
using Microsoft.Xna.Framework;

namespace Frtal.Wayfinder.Util
{
	public static class Gw2CoordinateUtil
	{
		public const double InchesPerMeter = 39.3701;

		public static double BearingFromNorth(Vector2 v)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return Math.Atan2(v.X, 0f - v.Y);
		}

		public static double NormalizeAngle(double a)
		{
			while (a > Math.PI)
			{
				a -= Math.PI * 2.0;
			}
			while (a <= -Math.PI)
			{
				a += Math.PI * 2.0;
			}
			return a;
		}

		public static double RelativeBearing(Vector2 playerCont, Vector2 targetCont, Vector2 facing)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return NormalizeAngle(BearingFromNorth(targetCont - playerCont) - BearingFromNorth(facing));
		}

		public static float BearingToOffsetX(double relativeBearing, double halfFovRadians, float halfBarWidth)
		{
			if (halfFovRadians <= 0.0)
			{
				return 0f;
			}
			if (halfFovRadians < 1.45)
			{
				double denom = Math.Tan(halfFovRadians);
				if (denom > 1E-06)
				{
					return (float)(Math.Tan(relativeBearing) / denom) * halfBarWidth;
				}
			}
			return (float)(relativeBearing / halfFovRadians) * halfBarWidth;
		}
	}
}
