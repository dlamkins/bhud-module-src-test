using System.Collections.Generic;
using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Strikes.Models
{
	public class StrikeMission : EncounterInterface
	{
		[JsonProperty("mapIds")]
		public List<int> MapIds = new List<int>();
	}
}
