using System.Collections.Generic;
using Newtonsoft.Json;

namespace CinemaModule.Models
{
	public class SavedStreamCollection
	{
		[JsonProperty("streams")]
		public List<SavedStream> Streams { get; set; } = new List<SavedStream>();

	}
}
