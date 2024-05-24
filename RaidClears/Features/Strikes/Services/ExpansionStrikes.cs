using System.Collections.Generic;
using Newtonsoft.Json;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Strikes.Services
{
	public class ExpansionStrikes
	{
		[JsonProperty("id")]
		public string Id = "undefined";

		[JsonProperty("name")]
		public string Name = "undefined";

		[JsonProperty("abbriviation")]
		public string Abbriviation = "undefined";

		[JsonProperty("asset")]
		public string asset = "missing.png";

		[JsonProperty("resets")]
		public string Resets = "weekly";

		[JsonProperty("daily_priority_modulo")]
		public int DailyPriorityModulo = 1;

		[JsonProperty("daily_priority_offset")]
		public int DailyPriorityOffset;

		[JsonProperty("missions")]
		public List<StrikeMission> Missions = new List<StrikeMission>();

		public List<BoxModel> GetEncounters()
		{
			List<BoxModel> missionslist = new List<BoxModel>();
			foreach (StrikeMission mission in Missions)
			{
				missionslist.Add(new Encounter(mission));
			}
			return missionslist;
		}
	}
}
