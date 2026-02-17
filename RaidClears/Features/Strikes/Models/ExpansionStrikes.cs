using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Strikes.Models
{
	[Serializable]
	public class ExpansionStrikes : EncounterInterface, IExpansion<BossEncounter>
	{
		[JsonProperty("asset")]
		public string asset = "missing.png";

		[JsonProperty("resets")]
		public string Resets = "weekly";

		[JsonProperty("daily_priority_modulo")]
		public int DailyPriorityModulo = 1;

		[JsonProperty("daily_priority_offset")]
		public int DailyPriorityOffset;

		[JsonProperty("missions")]
		public List<BossEncounter> Missions = new List<BossEncounter>();

		[JsonIgnore]
		public string Asset => asset;

		[JsonIgnore]
		public IReadOnlyList<BossEncounter> Children => Missions;

		string IExpansion<BossEncounter>.Id => Id;

		public List<BoxModel> GetEncounters()
		{
			List<BoxModel> missionslist = new List<BoxModel>();
			foreach (BossEncounter mission in Missions)
			{
				missionslist.Add(new Encounter(mission, isStrike: true));
			}
			return missionslist;
		}

		public BossEncounter ToBossEncounter()
		{
			return new BossEncounter
			{
				Name = Name,
				Id = Id,
				Abbriviation = Abbriviation
			};
		}
	}
}
