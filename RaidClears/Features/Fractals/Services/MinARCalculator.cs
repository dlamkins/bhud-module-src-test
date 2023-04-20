using System;

namespace RaidClears.Features.Fractals.Services
{
	public static class MinARCalculator
	{
		public static int MinArForScale(int scale)
		{
			if (scale < 20)
			{
				return 0;
			}
			return (int)Math.Ceiling(((float)scale * 0.02136f - 0.33f - 0.01f) / 0.012f);
		}
	}
}
