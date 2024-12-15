using System;
using Newtonsoft.Json;

namespace RaidClears.Features.Shared.Models
{
	[Serializable]
	public class EncounterInterface
	{
		[JsonProperty("id")]
		public string Id = "undefined";

		[JsonProperty("name")]
		public string Name = "undefined";

		[JsonProperty("abbriviation")]
		public string Abbriviation = "undefined";

		[JsonProperty("assetId")]
		public int AssetId;
	}
}
