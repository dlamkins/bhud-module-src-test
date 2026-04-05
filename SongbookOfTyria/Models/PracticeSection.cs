using Newtonsoft.Json;

namespace SongbookOfTyria.Models
{
	public class PracticeSection
	{
		[JsonProperty("label")]
		public string Label { get; set; }

		[JsonProperty("bar")]
		public int Bar { get; set; }
	}
}
