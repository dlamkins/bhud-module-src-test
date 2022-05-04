using System;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Controls
{
	internal class EnumProvider<T> : ControlProvider<T, T> where T : Enum
	{
		public override Control CreateControl(BoxedValue<T> value, Func<T, bool> isEnabled, Func<T, bool> isValid, (float Min, float Max)? range, int width, int height, int x, int y)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected O, but got Unknown
			Dropdown val = new Dropdown();
			((Control)val).set_Width(width);
			((Control)val).set_Location(new Point(x, y));
			BoxedValue<T> boxedValue = value;
			val.set_SelectedItem((boxedValue != null) ? boxedValue.Value.ToString() : null);
			((Control)val).set_Enabled(isEnabled?.Invoke(value.Value) ?? true);
			Dropdown dropdown = val;
			string[] names = Enum.GetNames(typeof(T));
			foreach (string enumValue in names)
			{
				dropdown.get_Items().Add(enumValue);
			}
			if (value != null)
			{
				bool resetingValue = false;
				dropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object s, ValueChangedEventArgs e)
				{
					if (!resetingValue)
					{
						T val2 = (T)Enum.Parse(typeof(T), e.get_CurrentValue());
						if (isValid?.Invoke(val2) ?? true)
						{
							value.Value = val2;
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
