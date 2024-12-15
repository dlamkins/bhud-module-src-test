using System.Collections.Generic;
using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Fractals.Services
{
	public class FractalMap
	{
		[JsonProperty("label")]
		public string Label = "undefined";

		[JsonProperty("short")]
		public string ShortLabel = "undefined";

		[JsonProperty("api")]
		public string ApiLabel = "undefined";

		[JsonProperty("scales")]
		public List<int> Scales = new List<int>();

		[JsonProperty("id")]
		public int MapId;

		public EncounterInterface ToEncounterInterface()
		{
			return new EncounterInterface
			{
				Id = ApiLabel,
				Name = Label,
				Abbriviation = ShortLabel
			};
		}
	}
}
