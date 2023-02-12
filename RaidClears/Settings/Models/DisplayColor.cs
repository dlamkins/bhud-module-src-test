using Blish_HUD.Settings;

namespace RaidClears.Settings.Models
{
	public class DisplayColor
	{
		public SettingEntry<string> NotCleared { get; set; }

		public SettingEntry<string> Cleared { get; set; }

		public SettingEntry<string> Text { get; set; }

		public SettingEntry<string> Background { get; set; }
	}
}
