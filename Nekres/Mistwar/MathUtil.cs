using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace Nekres.Mistwar
{
	internal static class MathUtil
	{
		public const double DegToRad = Math.PI / 180.0;

		public const double RadToDeg = 180.0 / Math.PI;

		public const double AngleToRad = Math.PI;

		public const double RadToAngle = 1.0 / Math.PI;

		public const double AngleToDeg = 180.0;

		public const double DegToAngle = 0.005555555555555556;

		public const float MetersToInches = 39.37008f;

		public const float InchesToMeters = 0.0254f;

		public static double Squared(double x)
		{
			return x * x;
		}

		public static double Cubed(double x)
		{
			return x * x * x;
		}

		public static double Biquadrated(double x)
		{
			return x * x * x * x;
		}

		public static float Squared(float x)
		{
			return x * x;
		}

		public static double Clamp(double x, double min, double max)
		{
			return Math.Min(Math.Max(x, min), max);
		}

		public static float Clamp(float x, float min, float max)
		{
			return Math.Min(Math.Max(x, min), max);
		}

		public static double Clamp01(double x)
		{
			return Clamp(x, 0.0, 1.0);
		}

		public static double InverseLerp(double min, double max, double x, bool clamp = false)
		{
			return ((clamp ? Clamp(x, min, max) : x) - min) / (max - min);
		}

		public static double Lerp(double min, double max, double x, bool clamp = false)
		{
			return min + (max - min) * (clamp ? Clamp01(x) : x);
		}

		public static double Scale(double x, double sourceMin, double sourceMax, double targetMin, double targetMax, bool clamp = false)
		{
			return Lerp(targetMin, targetMax, InverseLerp(sourceMin, sourceMax, x), clamp);
		}

		public static float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
		{
			return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Cross(Vector2 a, Vector2 b)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			return a.X * b.Y - a.Y * b.X;
		}

		public static int RoundToClosest(int value, int a, int b)
		{
			if (Math.Abs(value - a) >= Math.Abs(value - b))
			{
				return b;
			}
			return a;
		}
	}
}
