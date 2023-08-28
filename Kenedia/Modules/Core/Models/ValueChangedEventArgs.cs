using System;

namespace Kenedia.Modules.Core.Models
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
