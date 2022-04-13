using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Extensions;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Settings.Controls
{
	internal class TextBoxProvider : ControlProvider<string>
	{
		internal override Control CreateControl(SettingEntry<string> settingEntry, Func<SettingEntry<string>, string, bool> validationFunction, int width, int heigth, int x, int y)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected O, but got Unknown
			TextBox val = new TextBox();
			((Control)val).set_Width(width);
			((Control)val).set_Location(new Point(x, y));
			((TextInputBase)val).set_Text(settingEntry?.get_Value() ?? string.Empty);
			((Control)val).set_Enabled(!((SettingEntry)(object)settingEntry).IsDisabled());
			TextBox textBox = val;
			if (settingEntry != null)
			{
				((TextInputBase)textBox).add_TextChanged((EventHandler<EventArgs>)delegate(object s, EventArgs e)
				{
					ValueChangedEventArgs<string> val2 = (ValueChangedEventArgs<string>)(object)e;
					if (validationFunction?.Invoke(settingEntry, val2.get_NewValue()) ?? false)
					{
						settingEntry.set_Value(val2.get_NewValue());
					}
					else
					{
						((TextInputBase)textBox).set_Text(val2.get_PreviousValue());
					}
				});
			}
			return (Control)(object)textBox;
		}
	}
}
