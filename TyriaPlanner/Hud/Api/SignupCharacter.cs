using Newtonsoft.Json;

namespace TyriaPlanner.Hud.Api
{
	public sealed class SignupCharacter
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("profession")]
		public string Profession { get; set; }

		[JsonProperty("eliteSpec")]
		public string EliteSpec { get; set; }
	}
}
