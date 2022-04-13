using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Extensions;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Settings.Controls
{
	internal class KeybindingProvider : ControlProvider<KeyBinding>
	{
		internal override Control CreateControl(SettingEntry<KeyBinding> settingEntry, Func<SettingEntry<KeyBinding>, KeyBinding, bool> validationFunction, int width, int heigth, int x, int y)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			KeybindingAssigner keybindingAssigner2 = new KeybindingAssigner(settingEntry.get_Value(), withName: false);
			((Control)keybindingAssigner2).set_Width(width);
			((Control)keybindingAssigner2).set_Location(new Point(x, y));
			((Control)keybindingAssigner2).set_Enabled(!((SettingEntry)(object)settingEntry).IsDisabled());
			KeybindingAssigner keybindingAssigner = keybindingAssigner2;
			if (settingEntry != null)
			{
				keybindingAssigner.BindingChanged += delegate
				{
					if (validationFunction?.Invoke(settingEntry, keybindingAssigner.KeyBinding) ?? false)
					{
						settingEntry.set_Value(keybindingAssigner.KeyBinding);
					}
					else
					{
						keybindingAssigner.KeyBinding = settingEntry.get_Value();
					}
				};
			}
			return (Control)(object)keybindingAssigner;
		}
	}
}
