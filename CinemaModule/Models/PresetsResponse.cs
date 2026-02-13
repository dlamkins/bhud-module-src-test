using System.Collections.Generic;
using Newtonsoft.Json;

namespace CinemaModule.Models
{
	public class PresetsResponse
	{
		[JsonProperty("worldLocations")]
		public List<WorldLocationPresetData> WorldLocations { get; set; } = new List<WorldLocationPresetData>();


		[JsonProperty("streams")]
		public List<StreamPresetData> Streams { get; set; } = new List<StreamPresetData>();


		[JsonProperty("twitchChannels")]
		public List<string> TwitchChannels { get; set; } = new List<string>();

	}
}
