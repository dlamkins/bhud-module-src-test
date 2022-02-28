using Newtonsoft.Json;

namespace KillProofModule.Models
{
	public class Miniature
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("icon")]
		public string Icon { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}
}
