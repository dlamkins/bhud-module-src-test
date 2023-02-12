using System.Collections.Generic;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Enums;

namespace RaidClears.Features.Strikes.Services
{
	public class StrikeInfo
	{
		public Encounter Encounter;

		public Encounters.StrikeMission TomorrowEncounter;

		public List<int> MapIds;

		public StrikeInfo(Encounters.StrikeMission mission, List<int> maps, Encounters.StrikeMission tomorrow)
		{
			Encounter = new Encounter(mission);
			MapIds = maps;
			TomorrowEncounter = tomorrow;
		}
	}
}
