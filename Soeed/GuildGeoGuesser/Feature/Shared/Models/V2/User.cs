using System;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	[Serializable]
	public class User
	{
		[JsonProperty("AccountName")]
		public string AccountName { get; set; } = "";


		[JsonProperty("createdAt")]
		public DateTime BannedOn { get; set; } = DateTime.Now;


		[JsonProperty("LastActivity")]
		public DateTime LastActivity { get; set; } = DateTime.Now;


		[JsonProperty("GuessCount")]
		public int GuessCount { get; set; }

		[JsonProperty("VoteCount")]
		public int VoteCount { get; set; }
	}
}
