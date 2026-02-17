using System.Collections.Generic;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Strikes.Services
{
	public class StrikeInfo
	{
		public Encounter Encounter;

		public BossEncounter TomorrowEncounter;

		public List<int> MapIds;

		public StrikeInfo(BossEncounter mission, List<int> maps, BossEncounter tomorrow)
		{
			Encounter = new Encounter(mission, isStrike: true);
			MapIds = maps;
			TomorrowEncounter = tomorrow;
		}

		public StrikeInfo(BossEncounter mission, BossEncounter tomorrow)
		{
			Encounter = new Encounter(mission, isStrike: true);
			MapIds = mission.MapIds;
			TomorrowEncounter = tomorrow;
		}
	}
}
