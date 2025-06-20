using System;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	[Serializable]
	public class GuessCreate
	{
		[JsonProperty("AccountName")]
		public string AccountName { get; set; } = "";


		[JsonProperty("Location")]
		public Location Location { get; set; } = new Location();

	}
}
