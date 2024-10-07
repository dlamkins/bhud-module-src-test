using System;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class PropertyAndValueChangedEventArgs : EventArgs
	{
		public string PropertyName { get; set; }

		public object OldValue { get; set; }

		public object NewValue { get; set; }

		public PropertyAndValueChangedEventArgs(string propertyName, object oldValue, object newValue)
		{
			PropertyName = propertyName;
			OldValue = oldValue;
			NewValue = newValue;
		}
	}
}
