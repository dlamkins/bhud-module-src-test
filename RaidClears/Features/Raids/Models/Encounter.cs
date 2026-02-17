using RaidClears.Features.Fractals.Services;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Raids.Models
{
	public class Encounter : BoxModel
	{
		public Encounter(BossEncounter enc, bool isStrike)
			: base(enc.EncounterId, enc.Name, isStrike ? Service.StrikeSettings.GetEncounterLabel(enc) : Service.RaidSettings.GetEncounterLabel(enc))
		{
			_tooltip = new RaidTooltipView
			{
				Encounter = enc
			};
		}

		public Encounter(FractalMap fractal)
			: base(fractal.ApiLabel, fractal.Label, fractal.ShortLabel)
		{
		}
	}
}
