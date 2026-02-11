using System;
using Newtonsoft.Json;

namespace RaidClears.Features.Shared.Models
{
	[Serializable]
	public class BountyEncounterReference
	{
		[JsonProperty("encounterId")]
		public string EncounterId { get; set; } = "";


		[JsonProperty("type")]
		public string Type { get; set; } = "raid_encounter";

	}
}
