using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Models.BlishHudAPI
{
	public class APIError
	{
		[JsonProperty("message")]
		public string Message { get; set; }
	}
}
