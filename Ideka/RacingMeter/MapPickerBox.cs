namespace Ideka.RacingMeter
{
	public class MapPickerBox : ValueControl<int, int, MapPicker>
	{
		protected override void Initialize(MapPicker control)
		{
			control.ValueChanged += delegate
			{
				if (!TryMakeValue(control.Value, out var value))
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

		protected override bool TryReflectValue(ref int value, MapPicker control)
		{
			if (value <= 0)
			{
				return false;
			}
			control.Value = value;
			return true;
		}

		public MapPickerBox()
			: base(initialize: true)
		{
		}
	}
}
