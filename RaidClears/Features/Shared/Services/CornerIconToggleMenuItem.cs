using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using RaidClears.Localization;

namespace RaidClears.Features.Shared.Services
{
	public class CornerIconToggleMenuItem : ContextMenuStripItem
	{
		public CornerIconToggleMenuItem(SettingEntry<bool> setting, string displayLabel)
		{
			SettingEntry<bool> setting2 = setting;
			((ContextMenuStripItem)this)._002Ector(displayLabel);
			CornerIconToggleMenuItem cornerIconToggleMenuItem = this;
			string baseText = displayLabel;
			((ContextMenuStripItem)this).set_Text((setting2.get_Value() ? Strings.VisibleHide : Strings.VisibleShow) + " " + baseText);
			((Control)this).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				setting2.set_Value(!setting2.get_Value());
			});
			setting2.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				((ContextMenuStripItem)cornerIconToggleMenuItem).set_Text((setting2.get_Value() ? Strings.VisibleHide : Strings.VisibleShow) + " " + baseText);
			});
		}

		public CornerIconToggleMenuItem(Control control, string displayLabel)
		{
			Control control2 = control;
			((ContextMenuStripItem)this)._002Ector(displayLabel);
			((Control)this).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				control2.Show();
			});
		}
	}
}
