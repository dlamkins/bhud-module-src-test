using Newtonsoft.Json;

namespace Gorthax.Gilledwars
{
	public class AcceptedKey
	{
		[JsonProperty("itemId")]
		public int ItemId { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }
	}
}
