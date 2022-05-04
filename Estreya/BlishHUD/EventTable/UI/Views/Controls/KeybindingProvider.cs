using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Estreya.BlishHUD.EventTable.Controls;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Controls
{
	internal class KeybindingProvider : ControlProvider<KeyBinding, KeyBinding>
	{
		public override Control CreateControl(BoxedValue<KeyBinding> value, Func<KeyBinding, bool> isEnabled, Func<KeyBinding, bool> isValid, (float Min, float Max)? range, int width, int heigth, int x, int y)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			KeybindingAssigner keybindingAssigner2 = new KeybindingAssigner(value.Value, withName: false);
			((Control)keybindingAssigner2).set_Width(width);
			((Control)keybindingAssigner2).set_Location(new Point(x, y));
			((Control)keybindingAssigner2).set_Enabled(isEnabled?.Invoke(value?.Value) ?? true);
			KeybindingAssigner keybindingAssigner = keybindingAssigner2;
			if (value != null)
			{
				keybindingAssigner.BindingChanged += delegate
				{
					if (isValid?.Invoke(keybindingAssigner.KeyBinding) ?? true)
					{
						value.Value = keybindingAssigner.KeyBinding;
					}
					else
					{
						keybindingAssigner.KeyBinding = value.Value;
					}
				};
			}
			return (Control)(object)keybindingAssigner;
		}
	}
}
