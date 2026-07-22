using System;

namespace Kenedia.Modules.Characters.Services
{
	public sealed class StateVarChangedEventArgs<T> : EventArgs
	{
		public T OldValue { get; }

		public T NewValue { get; }

		public StateVarChangedEventArgs(T oldValue, T newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}
	}
}
