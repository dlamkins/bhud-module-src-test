using System.Collections.Generic;
using Blish_HUD.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Settings.Models;

namespace RaidClears.Features.Fractals.Models
{
	public static class FractalMetaData
	{
		private static FractalSettings Settings => Service.Settings.FractalSettings;

		public static IEnumerable<Fractal> Create(FractalsPanel panel)
		{
			return new List<Fractal>
			{
				new TierNFractals("Tier #", 0, "T#", new List<BoxModel>(), (Container)(object)panel),
				new DailyFractal("Daily Recommended", 1, "Rec", new List<BoxModel>(), (Container)(object)panel),
				new TierNTomorrow("Tomorrow T#", 1, "Tom", new List<BoxModel>(), (Container)(object)panel)
			};
		}
	}
}
