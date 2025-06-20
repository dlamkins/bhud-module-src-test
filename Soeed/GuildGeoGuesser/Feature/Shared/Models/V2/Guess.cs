using System;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	[Serializable]
	public class Guess
	{
		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; } = DateTime.Now;


		[JsonProperty("Location")]
		public Location Location { get; set; } = new Location();


		[JsonProperty("Distance")]
		public float Distance { get; set; }
	}
}
