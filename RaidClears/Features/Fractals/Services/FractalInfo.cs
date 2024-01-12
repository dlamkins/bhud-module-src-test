using System.Collections.Generic;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Enums;

namespace RaidClears.Features.Fractals.Services
{
	public class FractalInfo
	{
		public Encounter Encounter;

		public Encounters.Fractal TomorrowEncounter;

		public List<string>? Instabilities;

		public List<string>? TomorrowInstabilities;

		public FractalInfo(Encounters.Fractal mission, Encounters.Fractal tomorrow)
		{
			Encounter = new Encounter(mission);
			TomorrowEncounter = tomorrow;
		}

		public FractalInfo(Encounters.Fractal mission, Encounters.Fractal tomorrow, List<string> instab, List<string> tomorrowInstab)
		{
			Encounter = new Encounter(mission);
			TomorrowEncounter = tomorrow;
			Instabilities = instab;
			TomorrowInstabilities = tomorrowInstab;
		}
	}
}
