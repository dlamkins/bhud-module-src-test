using System;

namespace Kenedia.Modules.Core.Models
{
	public class ValueChangedEventArgs<TValue> : EventArgs
	{
		public string PropertyName { get; set; } = string.Empty;


		public TValue? OldValue { get; set; }

		public TValue? NewValue { get; set; }

		public ValueChangedEventArgs(TValue? oldValue, TValue? newValue, string? propertyName = null)
		{
			PropertyName = propertyName ?? string.Empty;
			OldValue = oldValue;
			NewValue = newValue;
		}
	}
}
