using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Kenedia.Modules.Core.Models
{
	public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	{
		public new TValue this[TKey key]
		{
			get
			{
				return base[key];
			}
			set
			{
				OnValueChanged(key, ContainsKey(key) ? base[key] : default(TValue), value, "Item");
			}
		}

		public event PropertyChangedEventHandler? CollectionChanged;

		public event EventHandler<DictionaryItemChangedEventArgs<TKey, TValue?>>? ItemChanged;

		private void OnValueChanged(TKey key, TValue? oldValue, TValue? newValue, [CallerMemberName] string? propName = null)
		{
			if ((!(oldValue?.Equals(newValue))) ?? true)
			{
				bool num = ContainsKey(key);
				base[key] = newValue;
				this.ItemChanged?.Invoke(this, new DictionaryItemChangedEventArgs<TKey, TValue>(key, oldValue, newValue));
				if (!num)
				{
					this.CollectionChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
				}
			}
		}

		public new void Add(TKey key, TValue? value)
		{
			if (!ContainsKey(key))
			{
				base.Add(key, value);
				this.ItemChanged?.Invoke(this, new DictionaryItemChangedEventArgs<TKey, TValue>(key, default(TValue), value));
				this.CollectionChanged?.Invoke(this, new PropertyChangedEventArgs("Collection"));
			}
		}

		public new bool Remove(TKey key)
		{
			bool num = base.Remove(key);
			if (num)
			{
				PropertyChangedEventHandler? collectionChanged = this.CollectionChanged;
				if (collectionChanged == null)
				{
					return num;
				}
				collectionChanged!(this, new PropertyChangedEventArgs("Collection"));
			}
			return num;
		}

		public void Wipe()
		{
			foreach (TKey key in base.Keys.ToList())
			{
				this[key] = default(TValue);
			}
		}
	}
}
