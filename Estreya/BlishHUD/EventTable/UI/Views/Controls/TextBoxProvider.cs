using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Controls
{
	internal class TextBoxProvider : ControlProvider<string, string>
	{
		public override Control CreateControl(BoxedValue<string> value, Func<string, bool> isEnabled, Func<string, bool> isValid, (float Min, float Max)? range, int width, int height, int x, int y)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Expected O, but got Unknown
			TextBox val = new TextBox();
			((Control)val).set_Width(width);
			((Control)val).set_Location(new Point(x, y));
			((TextInputBase)val).set_Text(value?.Value ?? string.Empty);
			((Control)val).set_Enabled(isEnabled?.Invoke(value?.Value) ?? true);
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
						value.Value = val2.get_NewValue();
					}
					else
					{
						int cursorIndex = ((TextInputBase)textBox).get_CursorIndex() - 1;
						((TextInputBase)textBox).set_Text(val2.get_PreviousValue());
						((TextInputBase)textBox).set_CursorIndex(cursorIndex);
					}
				});
			}
			return (Control)(object)textBox;
		}
	}
}
