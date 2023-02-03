using System;
using Blish_HUD;
using Blish_HUD.Controls;

namespace Ideka.RacingMeter
{
	public abstract class ValueTextBox<TValue> : ValueControl<TValue, string, TextBox>
	{
		public ValueTextBox(TValue start)
			: base(start)
		{
			((TextInputBase)base.Control).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				if (TryMakeValue(((TextInputBase)base.Control).get_Text(), out var value3))
				{
					SetTempValue(value3, reflect: false);
				}
			});
			((TextInputBase)base.Control).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate
			{
				if (!((TextInputBase)base.Control).get_Focused())
				{
					if (!TryMakeValue(((TextInputBase)base.Control).get_Text(), out var value))
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
						base.Value = value;
					}
				}
			});
		}

		protected sealed override bool TryReflectValue(ref TValue value)
		{
			if (!TryMakeText(ref value, out var text))
			{
				return false;
			}
			((TextInputBase)base.Control).set_Text(text);
			return true;
		}

		protected abstract bool TryMakeText(ref TValue value, out string text);
	}
}
