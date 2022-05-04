using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Controls
{
	internal class TimeSpanProvider : ControlProvider<TimeSpan, string>
	{
		public override Control CreateControl(BoxedValue<TimeSpan> value, Func<TimeSpan, bool> isEnabled, Func<string, bool> isValid, (float Min, float Max)? range, int width, int heigth, int x, int y)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			TextBox val = new TextBox();
			((Control)val).set_Width(width);
			((Control)val).set_Location(new Point(x, y));
			((TextInputBase)val).set_Text(value?.Value.ToString());
			((Control)val).set_Enabled(isEnabled?.Invoke(value?.Value ?? TimeSpan.Zero) ?? true);
			TextBox textBox = val;
			if (value != null)
			{
				((TextInputBase)textBox).add_TextChanged((EventHandler<EventArgs>)delegate(object s, EventArgs e)
				{
					ValueChangedEventArgs<string> val2 = (ValueChangedEventArgs<string>)(object)e;
					bool flag = true;
					if (range.HasValue && ((float)val2.get_NewValue().Length < range.Value.Min || (float)val2.get_NewValue().Length > range.Value.Max))
					{
						flag = false;
					}
					if (flag && (isValid?.Invoke(val2.get_NewValue()) ?? true))
					{
						TimeSpan.TryParse(val2.get_NewValue(), out var result);
						value.Value = result;
					}
				});
			}
			return (Control)(object)textBox;
		}
	}
}
