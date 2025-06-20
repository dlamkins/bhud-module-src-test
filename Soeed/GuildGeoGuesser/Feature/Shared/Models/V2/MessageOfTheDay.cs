using System;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	[Serializable]
	public class MessageOfTheDay
	{
		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("AccountName")]
		public string AccountName { get; set; } = "";


		[JsonProperty("Message")]
		public string Message { get; set; } = "";


		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; } = DateTime.Now;

	}
}
