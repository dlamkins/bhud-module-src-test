using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;

namespace FarmingTracker
{
	public class SettingControl : ViewContainer
	{
		public SettingControl(Container parent, SettingEntry settingEntry)
			: this()
		{
			((Control)this).set_Parent(parent);
			((ViewContainer)this).Show(SettingView.FromType(settingEntry, ((Control)parent).get_Width()));
		}
	}
}
