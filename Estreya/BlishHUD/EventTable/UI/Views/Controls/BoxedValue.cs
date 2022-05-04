using System;

namespace Estreya.BlishHUD.EventTable.UI.Views.Controls
{
	public class BoxedValue<T>
	{
		private T _value;

		public T Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				ValueUpdatedAction?.Invoke(_value);
			}
		}

		private Action<T> ValueUpdatedAction { get; }

		public BoxedValue(T value, Action<T> valueUpdatedAction)
		{
			Value = value;
			ValueUpdatedAction = valueUpdatedAction;
		}
	}
}
