using System.Collections.Generic;
using Blish_HUD.Controls;

namespace RaidClears.Features.Fractals.Models
{
	public static class FractalMetaData
	{
		public static IEnumerable<Fractal> Create(FractalsPanel panel)
		{
			return new List<Fractal>
			{
				new CMFractals((Container)(object)panel),
				new TierNTomorrow((Container)(object)panel),
				new TierNFractals((Container)(object)panel),
				new DailyFractal((Container)(object)panel)
			};
		}
	}
}
