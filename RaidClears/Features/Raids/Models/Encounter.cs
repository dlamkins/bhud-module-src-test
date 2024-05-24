using RaidClears.Features.Fractals.Services;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Services;

namespace RaidClears.Features.Raids.Models
{
	public class Encounter : BoxModel
	{
		public Encounter(Encounters.RaidBosses boss)
			: base(boss.GetApiLabel(), boss.GetLabel(), boss.GetLabelShort())
		{
		}

		public Encounter(StrikeMission boss)
			: base(boss.Id, boss.Name, boss.Abbriviation)
		{
		}

		public Encounter(FractalMap fractal)
			: base(fractal.ApiLabel, fractal.Label, fractal.ShortLabel)
		{
		}
	}
}
