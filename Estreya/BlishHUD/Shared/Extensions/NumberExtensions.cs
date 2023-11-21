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
	}
}
