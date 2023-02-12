using Blish_HUD;
using Blish_HUD.Settings;
using RaidClears.Settings.Enums;

namespace RaidClears.Settings.Models
{
	public class DisplayStyle
	{
		public SettingEntry<FontSize> FontSize { get; set; }

		public SettingEntry<LabelDisplay> LabelDisplay { get; set; }

		public SettingEntry<Layout> Layout { get; set; }

		public SettingEntry<float> LabelOpacity { get; set; }

		public SettingEntry<float> GridOpacity { get; set; }

		public SettingEntry<float> BgOpacity { get; set; }

		public DisplayColor Color { get; set; }
	}
}
