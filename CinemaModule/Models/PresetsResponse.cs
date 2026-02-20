using System.Collections.Generic;
using Newtonsoft.Json;

namespace CinemaModule.Models
{
	public class PresetsResponse
	{
		[JsonProperty("worldLocations")]
		public List<WorldLocationPresetData> WorldLocations { get; set; } = new List<WorldLocationPresetData>();


		[JsonProperty("streams")]
		public List<StreamCategory> StreamCategories { get; set; } = new List<StreamCategory>();

	}
}
