using System.Collections.Generic;
using RaidClears.Features.Raids.Models;

namespace RaidClears.Features.Strikes.Services
{
	public class StrikeInfo
	{
		public Encounter Encounter;

		public StrikeMission TomorrowEncounter;

		public List<int> MapIds;

		public StrikeInfo(StrikeMission mission, List<int> maps, StrikeMission tomorrow)
		{
			Encounter = new Encounter(mission);
			MapIds = maps;
			TomorrowEncounter = tomorrow;
		}

		public StrikeInfo(StrikeMission mission, StrikeMission tomorrow)
		{
			Encounter = new Encounter(mission);
			MapIds = mission.MapIds;
			TomorrowEncounter = tomorrow;
		}
	}
}
