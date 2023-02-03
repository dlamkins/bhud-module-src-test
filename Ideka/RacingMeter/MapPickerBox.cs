namespace Ideka.RacingMeter
{
	public class MapPickerBox : ValueControl<int, int, MapPicker>
	{
		public MapPickerBox()
			: base(0)
		{
			base.Control.ValueChanged += delegate
			{
				if (!TryMakeValue(base.Control.Value, out var value))
				{
					ResetValue();
				}
				else if (value != base.Value)
				{
					CommitValue(value);
				}
			};
		}

		protected override bool TryMakeValue(int innerValue, out int value)
		{
			value = innerValue;
			return innerValue > 0;
		}

		protected override bool TryReflectValue(ref int value)
		{
			if (value <= 0)
			{
				return false;
			}
			base.Control.Value = value;
			return true;
		}
	}
}
