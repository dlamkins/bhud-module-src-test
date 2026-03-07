using System.Collections.Generic;
using Newtonsoft.Json;

namespace CinemaModule.Models.Location
{
	public class SavedLocationCollection
	{
		[JsonProperty("locations")]
		public List<SavedLocation> Locations { get; set; } = new List<SavedLocation>();

	}
}
