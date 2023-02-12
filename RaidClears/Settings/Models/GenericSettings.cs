using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace RaidClears.Settings.Models
{
	public class GenericSettings
	{
		public SettingEntry<bool> Enabled { get; set; }

		public SettingEntry<bool> PositionLock { get; set; }

		public SettingEntry<Point> Location { get; set; }

		public SettingEntry<bool> Tooltips { get; set; }

		public SettingEntry<bool> ToolbarIcon { get; set; }

		public SettingEntry<bool> Visible { get; set; }

		public SettingEntry<KeyBinding> ShowHideKeyBind { get; set; }

		public void ToggleVisible()
		{
			if (ToolbarIcon.get_Value())
			{
				Visible.set_Value(!Visible.get_Value());
			}
		}
	}
}
