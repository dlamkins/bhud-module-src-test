using System;

namespace Manlaan.CommanderMarkers
{
	public static class MathUtils
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
	}
}
