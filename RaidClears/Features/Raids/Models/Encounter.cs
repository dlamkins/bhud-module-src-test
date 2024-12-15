using RaidClears.Features.Fractals.Services;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Models;

namespace RaidClears.Features.Raids.Models
{
	public class Encounter : BoxModel
	{
		public Encounter(RaidEncounter enc)
			: base(enc.ApiId, enc.Name, Service.RaidSettings.GetEncounterLabel(enc))
		{
			_tooltip = new RaidTooltipView
			{
				Encoutner = enc
			};
		}

		public Encounter(StrikeMission boss)
			: base(boss.Id, boss.Name, boss.Abbriviation)
		{
			_tooltip = new RaidTooltipView
			{
				StrikeMission = boss
			};
		}

		public Encounter(FractalMap fractal)
			: base(fractal.ApiLabel, fractal.Label, fractal.ShortLabel)
		{
		}
	}
}
