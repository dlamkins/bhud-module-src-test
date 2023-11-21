using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Modules
{
	public struct ModuleValidationResponse
	{
		[JsonProperty("message")]
		public string Message { get; set; }
	}
}
