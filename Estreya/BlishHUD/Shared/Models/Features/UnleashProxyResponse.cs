using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Models.Features
{
	public class UnleashProxyResponse
	{
		[JsonProperty("toggles")]
		public UnleashProxyToggle[] Toggles { get; set; }
	}
}
