using System;

namespace RaidClears.Utils.Kenedia
{
	public class ValueChangedEventArgs<TValue> : EventArgs
	{
		public TValue? OldValue { get; set; }

		public TValue? NewValue { get; set; }

		public ValueChangedEventArgs(TValue? oldValue, TValue? newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}
	}
}
