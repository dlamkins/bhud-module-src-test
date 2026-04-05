using Newtonsoft.Json;

namespace SongbookOfTyria.Models
{
	public class MidiInstrument
	{
		[JsonProperty("number")]
		public int Number { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("family")]
		public string Family { get; set; }

		[JsonProperty("gw2_preset")]
		public int Gw2Preset { get; set; }

		[JsonProperty("gw2_name")]
		public string Gw2Name { get; set; }
	}
}
