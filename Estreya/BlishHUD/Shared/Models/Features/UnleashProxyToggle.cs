using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Models.Features
{
	public class UnleashProxyToggle
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("enabled")]
		public bool Enabled { get; set; }
	}
}
