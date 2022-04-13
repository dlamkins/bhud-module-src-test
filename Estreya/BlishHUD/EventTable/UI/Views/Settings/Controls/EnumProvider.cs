using System;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Extensions;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Settings.Controls
{
	internal class EnumProvider<T> : ControlProvider<T> where T : Enum
	{
		internal override Control CreateControl(SettingEntry<T> settingEntry, Func<SettingEntry<T>, T, bool> validationFunction, int width, int heigth, int x, int y)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			Dropdown val = new Dropdown();
			((Control)val).set_Width(width);
			((Control)val).set_Location(new Point(x, y));
			SettingEntry<T> obj = settingEntry;
			val.set_SelectedItem((obj != null) ? obj.get_Value().ToString() : null);
			((Control)val).set_Enabled(!((SettingEntry)(object)settingEntry).IsDisabled());
			Dropdown dropdown = val;
			string[] names = Enum.GetNames(((SettingEntry)settingEntry).get_SettingType());
			foreach (string enumValue in names)
			{
				dropdown.get_Items().Add(enumValue);
			}
			if (settingEntry != null)
			{
				bool resetingValue = false;
				dropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object s, ValueChangedEventArgs e)
				{
					if (!resetingValue)
					{
						T val2 = (T)Enum.Parse(((SettingEntry)settingEntry).get_SettingType(), e.get_CurrentValue());
						if (validationFunction?.Invoke(settingEntry, val2) ?? false)
						{
							settingEntry.set_Value(val2);
						}
						else
						{
							resetingValue = true;
							dropdown.set_SelectedItem(e.get_PreviousValue());
							resetingValue = false;
						}
					}
				});
			}
			return (Control)(object)dropdown;
		}
	}
}
