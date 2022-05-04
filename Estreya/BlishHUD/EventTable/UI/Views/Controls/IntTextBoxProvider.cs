using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Controls
{
	internal class IntTextBoxProvider : ControlProvider<int, string>
	{
		private static readonly Logger Logger = Logger.GetLogger<IntTextBoxProvider>();

		public override Control CreateControl(BoxedValue<int> value, Func<int, bool> isEnabled, Func<string, bool> isValid, (float Min, float Max)? range, int width, int heigth, int x, int y)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			TextBox val = new TextBox();
			((Control)val).set_Width(width);
			((Control)val).set_Location(new Point(x, y));
			((TextInputBase)val).set_Text((value?.Value ?? 50).ToString());
			TextBox textBox = val;
			((Control)textBox).set_Enabled(isEnabled?.Invoke(value?.Value ?? 50) ?? true);
			if (value != null)
			{
				((TextInputBase)textBox).add_TextChanged((EventHandler<EventArgs>)delegate(object s, EventArgs e)
				{
					ValueChangedEventArgs<string> val2 = (ValueChangedEventArgs<string>)(object)e;
					string text = (string.IsNullOrWhiteSpace(val2.get_NewValue()) ? "0" : val2.get_NewValue());
					int result;
					bool flag = int.TryParse(text, out result);
					Logger.Debug("Value \"{0}\" could be parsed: {1}", new object[2] { text, flag });
					if (!flag)
					{
						Logger.Debug("Range check not available.");
					}
					bool flag2 = true;
					if (range.HasValue && flag && ((float)result < range.Value.Min || (float)result > range.Value.Max))
					{
						flag2 = false;
					}
					if (flag2 && (isValid?.Invoke(text) ?? true))
					{
						int.TryParse(text, out var result2);
						value.Value = result2;
					}
				});
			}
			return (Control)(object)textBox;
		}
	}
}
