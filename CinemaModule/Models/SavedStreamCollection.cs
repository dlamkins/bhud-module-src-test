using System.Collections.Generic;
using Newtonsoft.Json;

namespace CinemaModule.Models
{
	public class SavedStreamCollection
	{
		[JsonProperty("streams")]
		public List<SavedStream> Streams { get; set; } = new List<SavedStream>();


		[JsonProperty("tabs")]
		public List<CustomStreamTab> Tabs { get; set; } = new List<CustomStreamTab>();

	}
}
