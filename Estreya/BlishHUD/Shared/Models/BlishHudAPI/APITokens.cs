using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Models.BlishHudAPI
{
	public struct APITokens
	{
		[JsonProperty("accessToken")]
		public string AccessToken { get; set; }

		[JsonProperty("refreshToken")]
		public string RefreshToken { get; set; }
	}
}
