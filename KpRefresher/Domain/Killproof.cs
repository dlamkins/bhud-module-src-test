using Newtonsoft.Json;

namespace KpRefresher.Domain
{
	public class Killproof
	{
		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("id")]
		public int Id { get; set; }
	}
}
