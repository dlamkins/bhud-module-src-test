using System.Collections.Generic;
using Newtonsoft.Json;

namespace CinemaModule.Models
{
	public class PresetsResponse
	{
		[JsonProperty("worldLocations")]
		public List<WorldLocationCategory> WorldLocationCategories { get; set; } = new List<WorldLocationCategory>();


		[JsonProperty("streams")]
		public List<StreamCategory> StreamCategories { get; set; } = new List<StreamCategory>();

	}
}
