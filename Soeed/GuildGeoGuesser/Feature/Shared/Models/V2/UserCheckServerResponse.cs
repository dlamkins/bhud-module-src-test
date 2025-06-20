using System;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	[Serializable]
	public class UserCheckServerResponse
	{
		[JsonProperty("MOTD")]
		public MessageOfTheDay Motd { get; set; } = new MessageOfTheDay();


		[JsonProperty("VersionCheck")]
		public bool VersionCheck { get; set; } = true;


		[JsonProperty("AccountName")]
		public string Account { get; set; } = "";


		[JsonProperty("Banned")]
		public bool Banned { get; set; }

		[JsonProperty("Ban")]
		public Ban Ban { get; set; } = new Ban();

	}
}
