using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Kenedia.Modules.Core.Models
{
	public class NotifyPropertyChangedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IDisposable where TKey : notnull where TValue : INotifyPropertyChanged
	{
		public new TValue? this[TKey key]
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

		public event EventHandler<PropertyChangedEventArgs>? CollectionChanged;

		public event EventHandler<PropertyChangedEventArgs>? ItemPropertyChanged;

		public event EventHandler<DictionaryItemChangedEventArgs<TKey, TValue?>>? ItemChanged;

		private void OnValueChanged(TKey key, TValue? oldValue, TValue? newValue, [CallerMemberName] string? propName = null)
		{
			if ((!(oldValue?.Equals(newValue))) ?? true)
			{
				if (oldValue != null)
				{
					oldValue!.PropertyChanged -= ItemProperty_Changed;
				}
				if (newValue != null)
				{
					newValue!.PropertyChanged += ItemProperty_Changed;
				}
				bool num = ContainsKey(key);
				base[key] = newValue;
				this.ItemChanged?.Invoke(this, new DictionaryItemChangedEventArgs<TKey, TValue>(key, oldValue, newValue));
				if (!num)
				{
					this.CollectionChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
				}
			}
		}

		private void ItemProperty_Changed(object? sender, PropertyChangedEventArgs e)
		{
			this.ItemPropertyChanged?.Invoke(sender, e);
		}

		public new void Add(TKey key, TValue? value)
		{
			if (!ContainsKey(key))
			{
				if (value != null)
				{
					value!.PropertyChanged += ItemProperty_Changed;
				}
				base.Add(key, value);
				this.ItemChanged?.Invoke(this, new DictionaryItemChangedEventArgs<TKey, TValue>(key, default(TValue), value));
				this.CollectionChanged?.Invoke(this, new PropertyChangedEventArgs("Collection"));
			}
		}

		public new bool Remove(TKey key)
		{
			if (ContainsKey(key) && this[key] != null)
			{
				TValue val = this[key];
				val.PropertyChanged -= ItemProperty_Changed;
			}
			return base.Remove(key);
		}

		public void Dispose()
		{
		}

		public virtual void Wipe()
		{
			foreach (TKey key in base.Keys.ToList())
			{
				this[key] = default(TValue);
			}
		}

		protected virtual void OnItemChanged()
		{
		}
	}
}
