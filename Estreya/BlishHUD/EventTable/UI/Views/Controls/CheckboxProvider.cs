using System;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Controls
{
	internal class CheckboxProvider : ControlProvider<bool, bool>
	{
		public override Control CreateControl(BoxedValue<bool> value, Func<bool, bool> isEnabled, Func<bool, bool> isValid, (float Min, float Max)? range, int width, int heigth, int x, int y)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			Checkbox val = new Checkbox();
			((Control)val).set_Width(width);
			((Control)val).set_Location(new Point(x, y));
			val.set_Checked(value?.Value ?? false);
			((Control)val).set_Enabled(isEnabled?.Invoke(value?.Value ?? false) ?? true);
			Checkbox checkbox = val;
			if (value != null)
			{
				checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
				{
					if (isValid?.Invoke(e.get_Checked()) ?? true)
					{
						value.Value = e.get_Checked();
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
