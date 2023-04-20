using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Enums;

namespace RaidClears.Features.Fractals.Services
{
	public class FractalInfo
	{
		public Encounter Encounter;

		public Encounters.Fractal TomorrowEncounter;

		public FractalInfo(Encounters.Fractal mission, Encounters.Fractal tomorrow)
		{
			Encounter = new Encounter(mission);
			TomorrowEncounter = tomorrow;
		}
	}
}
