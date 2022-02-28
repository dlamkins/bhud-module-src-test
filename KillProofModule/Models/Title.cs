using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KillProofModule.Models
{
	public class Title
	{
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty("mode")]
		public Mode Mode { get; set; }

		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}
}
