using Newtonsoft.Json;

namespace CinemaModule.Models
{
	public class StreamPresetData
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("avatar")]
		public string Avatar { get; set; }

		[JsonProperty("infoUrl")]
		public string InfoUrl { get; set; }
	}
}
