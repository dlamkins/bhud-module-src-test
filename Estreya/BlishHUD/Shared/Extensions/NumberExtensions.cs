namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class NumberExtensions
	{
		private const float METER_TO_INCHES_RATIO = 39.37008f;

		public static double ToInches(this double value)
		{
			return value * 39.370079040527344;
		}

		public static float ToInches(this float value)
		{
			return value * 39.37008f;
		}

		public static double ToMeters(this double value)
		{
			return value / 39.370079040527344;
		}

		public static float ToMeters(this float value)
		{
			return value / 39.37008f;
		}

		public static int Remap(this int from, int fromMin, int fromMax, int toMin, int toMax)
		{
			return (int)((float)from).Remap((float)fromMin, (float)fromMax, (float)toMin, (float)toMax);
		}

		public static float Remap(this float from, float fromMin, float fromMax, float toMin, float toMax)
		{
			return (float)((double)from).Remap((double)fromMin, (double)fromMax, (double)toMin, (double)toMax);
		}

		public static double Remap(this double from, double fromMin, double fromMax, double toMin, double toMax)
		{
			double num = from - fromMin;
			double fromMaxAbs = fromMax - fromMin;
			double normal = num / fromMaxAbs;
			return (toMax - toMin) * normal + toMin;
		}
	}
}
