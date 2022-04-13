using System;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Extensions;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Settings.Controls
{
	internal class CheckboxProvider : ControlProvider<bool>
	{
		internal override Control CreateControl(SettingEntry<bool> settingEntry, Func<SettingEntry<bool>, bool, bool> validationFunction, int width, int heigth, int x, int y)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			Checkbox val = new Checkbox();
			((Control)val).set_Width(width);
			((Control)val).set_Location(new Point(x, y));
			val.set_Checked(settingEntry?.get_Value() ?? false);
			((Control)val).set_Enabled(!((SettingEntry)(object)settingEntry).IsDisabled());
			Checkbox checkbox = val;
			if (settingEntry != null)
			{
				checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
				{
					if (validationFunction?.Invoke(settingEntry, e.get_Checked()) ?? false)
					{
						settingEntry.set_Value(e.get_Checked());
					}
					else
					{
						checkbox.set_Checked(!e.get_Checked());
					}
				});
			}
			return (Control)(object)checkbox;
		}
	}
}
