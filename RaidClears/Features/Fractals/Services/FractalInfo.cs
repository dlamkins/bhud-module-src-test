using System.Collections.Generic;
using RaidClears.Features.Raids.Models;

namespace RaidClears.Features.Fractals.Services
{
	public class FractalInfo
	{
		public Encounter Encounter;

		public FractalMap TomorrowEncounter;

		public List<string>? Instabilities;

		public List<string>? TomorrowInstabilities;

		public FractalInfo(FractalMap mission, FractalMap tomorrow)
		{
			Encounter = new Encounter(mission);
			TomorrowEncounter = tomorrow;
		}

		public FractalInfo(FractalMap mission, FractalMap tomorrow, List<string> instab, List<string> tomorrowInstab)
		{
			Encounter = new Encounter(mission);
			TomorrowEncounter = tomorrow;
			Instabilities = instab;
			TomorrowInstabilities = tomorrowInstab;
		}
	}
}
