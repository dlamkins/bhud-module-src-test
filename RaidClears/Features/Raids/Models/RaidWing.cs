using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Raids.Models
{
	[Serializable]
	public class RaidWing : EncounterInterface
	{
		[JsonProperty("number")]
		public int Number = -1;

		[JsonProperty("mapId")]
		public int MapId = -1;

		[JsonProperty("call_of_mists_timestamp")]
		public int CallOfTheMistsTimestamp = -1;

		[JsonProperty("call_of_mists_weeks")]
		public int CallOfTheMistsWeeks = -1;

		[JsonProperty("emboldened_timestamp")]
		public int EmboldenedTimestamp = -1;

		[JsonProperty("emboldened_weeks")]
		public int EmboldendedWeeks = -1;

		[JsonProperty("encounters")]
		public List<RaidEncounter> Encounters = new List<RaidEncounter>();

		public RaidEncounter ToRaidEncounter()
		{
			return new RaidEncounter
			{
				Name = Name,
				ApiId = Id,
				Abbriviation = Abbriviation
			};
		}
	}
}
