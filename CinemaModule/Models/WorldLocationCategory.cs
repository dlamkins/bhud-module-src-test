using System.Collections.Generic;
using Blish_HUD.Content;
using Newtonsoft.Json;

namespace CinemaModule.Models
{
	public class WorldLocationCategory
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("icon")]
		public string Icon { get; set; }

		[JsonProperty("locations")]
		public List<WorldLocationPresetData> Locations { get; set; } = new List<WorldLocationPresetData>();


		[JsonIgnore]
		public AsyncTexture2D IconTexture { get; set; }
	}
}
