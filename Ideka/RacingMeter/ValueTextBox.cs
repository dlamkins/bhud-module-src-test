using System;
using Blish_HUD;
using Blish_HUD.Controls;

namespace Ideka.RacingMeter
{
	public abstract class ValueTextBox<TValue> : ValueControl<TValue, string, TextBox>
	{
		protected override void Initialize(TextBox control)
		{
			((TextInputBase)control).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				if (TryMakeValue(((TextInputBase)control).get_Text(), out var value3))
				{
					SetTempValue(value3, reflect: false);
				}
			});
			((TextInputBase)control).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate
			{
				if (!((TextInputBase)control).get_Focused())
				{
					if (!TryMakeValue(((TextInputBase)control).get_Text(), out var value))
					{
						ResetValue();
					}
					else
					{
						TValue value2 = base.Value;
						if (value2 == null || !value2.Equals(value))
						{
							CommitValue(value);
						}
					}
				}
			});
		}

		protected sealed override bool TryReflectValue(ref TValue value, TextBox control)
		{
			if (!TryMakeText(ref value, out var text))
			{
				return false;
			}
			((TextInputBase)control).set_Text(text);
			return true;
		}

		protected abstract bool TryMakeText(ref TValue value, out string text);

		protected ValueTextBox()
			: base(initialize: true)
		{
		}
	}
}
