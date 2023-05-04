using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace Ideka.RacingMeter
{
	public class ButtonBox<TValue> : ValueControl<TValue, TValue, StandardButton>
	{
		public string ButtonText
		{
			get
			{
				return base.Control.get_Text();
			}
			set
			{
				base.Control.set_Text(value);
			}
		}

		public event EventHandler<MouseEventArgs> ButtonClick
		{
			add
			{
				((Control)base.Control).add_Click(value);
			}
			remove
			{
				((Control)base.Control).remove_Click(value);
			}
		}

		protected override bool TryMakeValue(TValue innerValue, out TValue value)
		{
			value = innerValue;
			return true;
		}

		protected override bool TryReflectValue(ref TValue value)
		{
			return true;
		}

		public ButtonBox(TValue start)
			: base(start)
		{
		}
	}
}
