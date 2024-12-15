using System;

namespace RaidClears.Utils.Kenedia
{
	public class DictionaryItemChangedEventArgs<TKey, TValue> : EventArgs
	{
		public TValue? OldValue { get; set; }

		public TValue? NewValue { get; set; }

		public TKey Key { get; set; }

		public DictionaryItemChangedEventArgs(TKey key, TValue? oldValue, TValue? newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
			Key = key;
		}
	}
}
