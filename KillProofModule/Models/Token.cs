using Newtonsoft.Json;

namespace KillProofModule.Models
{
	public class Token
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("icon")]
		public string Icon { get; set; }

		[JsonProperty("amount")]
		public int Amount { get; set; }
	}
}
