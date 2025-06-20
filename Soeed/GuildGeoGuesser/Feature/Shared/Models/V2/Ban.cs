using System;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	[Serializable]
	public class Ban
	{
		[JsonProperty("AccountName")]
		public string AccountName { get; set; } = "";


		[JsonProperty("Reason")]
		public string Reason { get; set; } = "";


		[JsonProperty("Moderator")]
		public string Moderator { get; set; } = "";


		[JsonProperty("createdAt")]
		public DateTime BannedOn { get; set; } = DateTime.Now;

	}
}
