using Blish_HUD.Settings;
using Newtonsoft.Json;

namespace falcon.cmtracker.Persistance
{
	public class Token
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("icon")]
		public string Icon { get; set; }

		public BossType bossType { get; set; }

		public SettingEntry<bool> setting { get; set; }
	}
}
